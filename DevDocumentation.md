# Developer Documentation

## 1. Přehled

Tento projekt provádí visualizaci vybraných řadících algoritmů. Visualizace i řazení jsou prováděny na jednom vlákně. Třídící algoritmy vrací v každém smysluplném kroku proměnnou "SortStep", ve které je napsáno, jaký typ operace a kde ho daný algoritmus dělá. Celou animaci řídí Timer, který každou milisekundu provede vykreslení.

Projekt se skládá ze tří hlavních částí:

- **`Form1`** — Zvládá UI a tok celé aplikace.
- **`VisualizationPanel`** — Komponenta, která obstarává samotné vykreslování
- **`SortingFunctions`** — statická třída, která obsahuje řadící algoritmy, každý vracející `IEnumerable<SortStep>`.

Pomocné typy a třídy: `AlgorithmInfo`, `SortStep`, `SortType`.

---

## 2. Jádro aplikace

### `SortType` (enum)
```csharp
public enum SortType { Begin, Compare, Swap, Done }
```
Pří komunikaci mezi třídící funkcí a visualizací předtavuje typ kroku, který třídící funkce provedla

### `SortStep`
```csharp
public class SortStep
{
    public int[] Array { get; set; }
    public SortType SortType { get; set; }
    public int? IndexA { get; set; }
    public int? IndexB { get; set; }
}
```
Páteřní komunikace mezi třídící funkcí, každý tento krok předtavuje jedno vykreslení. Proměnná Array se předává pouze jako reference, IndexA a IndexB mohou být null, protože ne vždycky změna odkazuje na dané dva prvky
### `AlgorithmInfo`
```csharp
public class AlgorithmInfo
{
    public string Name { get; set; }
    public Func<int[], IEnumerable<SortStep>> SortMethod { get; set; }
    public string Note { get; set; } = "";
}
```
Typ zajišťující předání samotného řadícího algoritmu UI. Přídání algoritmu je cíleně jednoduché, stačí jej pouze naprogramovat a přidat do seznamu algoritmů a dynamicky se pro něj vygeneruje tlačítko.

Proměnná Note je zatím nikde nepoužívaná, ale jestli si na to najdu čas tak ji tam dodělám.

---

## 3. Běh UI (state machine)

Každé tlačítko je dynamicky vygenerováno a při jeho zmáčnutí je opět zničeno, To je jedinná věc která řídí tok UI.

1. **`Form1_Load`** → `LoadInitializeButton()`
   Přidá Inicializační tlačítko

2. **`Initialize_Click`**
   Odstraní samo sebe. Následně vygeneruje pro každý vstup z algorithms tlačítko. Každé tlačítko má v proměnné Tag uloženého delegáta na řadící funkci, což pak zjednodušuje předání řadící funkce dál. 

3. **`SortButton_Click`**
   Odstraní všechna tlačítka. Následně přidá na obrazovku TextBox, Label a odevzdávací tlačítko. Do proměnné Tag v odevzdávacím tlačítku je zkopírován Tag kliknutého tlačítka. Tím se tedy posune delegát dál.

4. **`SubmitButton_Click`**
   - Zkusí zpracovat vstup do TextBoxu, jestliže se mu to nepodaří, bude to uživateli oznámeno a vrátí se na krok zpět.
   - Vygeneruje zamíchané pole zadané velikosti
   - Vygeneruje VisualizationPanel, velký jako klientská strana obrazovky
   - Vygeneruje si Enumerator<SortStep> z řadícího algoritmu
   - Vytvoří Timer a spustí ho (aktuálně s pevným intervalem 1 ms)
   - smaže všechny prvky na obrazovce

5. **`Timer_Tick`** (fires repeatedly while the sort runs)
   Pokud ještě existuje další krok v řadícím algoritmu, tak ho zobrazí, jinak se zabije.

6. **`Timer_Disposed`**
   Objeví "Back to the start" tlačítko

7. **`End_Click`**
   Zruší opravdu všechno na obrazovce (this.Controls.Clear()) (kdybych na něco v průběhu zapoměl, tak teď se to zruší) a načte krok 1.

---

## 4. Proč Timer

Najtěžší část projektu byla vytvořit samotnou animaci. První (marný) pokus byl použít foreach (protože tak se IEnumerable struktury procházejí). Velmi rychle jsem však zjistil, že nic nehraje a rychlé vyhledávání mi řeklo, že ani nikdy hrát nebude. Panel tetiž nevykresluje kdy se mu řekne, ale zařadí si to co má vykreslit do fronty a pak čeká až se uvolní vlákno. Protože však bude vlákno obsazené řadícím algoritmem až do jeho konce, tak se zobrazí pouze seřazené pole, což není očekávaný výsledek od visualizace třízení.

Po konzultaci s umělou inteligencí jsem si však poradil. Jednotlivé kroky se časují pomocí třídy Timer, která tiká každou milisekundu a tedy má vlákno volné na dost dlouho aby to stihla vykreslit.

---

## 5. `VisualizationPanel` rendering

Aby obraz neblikal (což byla vlastnost, která se mi velmi nelíbila) jsem dostal na internetu radu použít DoubleBuffering. Obyčejný Panel jej má nastavený na false a je to protected parameter. Musel jsem tedy vytvořit Subclass VisualizationPanel, kde jsem nastavil DoubleBuffering a přepsal celou logiku vykreslování do své nové podtřídy.

Sloupečky se kreslí po jednom. Kreslí se pomocí "using Brush ..." Tedy si alokuji nový štětec pro každý frame animace. Mohou mi tedy eventuálně dojít štětce (windows má nativně omezený počet štětců, který můžu používat), ale tím že je zase hned ruším (od toho keyword using), tak by je měl GC dostatečně rychle posbírat a na limit bych tedy narazit neměl. Nicméně o tomto problému vím a jsem schopný jej vyřešit alokováním si jednoho štětce na začátku a používáním ho behem běhu celého algoritmu.

Dále také barevnými štětci překresluji černě sloupečky barevně. Tedy se na daném místě nachází dva sloupečky přes sebe, do by ale neměl být problém jelikož tak nepřekresluji celé pole, takže se tím algoritmus zpomalí pouze o nakreslení dvou sloupečků.
---

## 6. Chyby

O těchto chybách vím, chyby jsem přijmul s tím, že jsem ochoten přijmout reklamaci a chyby opravit, ale nepřišlo mi že by to byl velký problém. Jestliže se později budu k projektu vracet, potom ty chyby opravím.

- rychlost animace je tvrdě dána. Je to otázka jednoho TextBoxu hned vedle počtu prvků v poli, ale napadlo mě to až když jsem měl všechno naprogramované.
- Jestliže uživatel změní velikost obrazovky, tak se nezmění velikost přehrávané části. Fakt nevím ani jak bych to chtěl dělat a byl by to další celý den strávený na projektu
- Note nikde nezobrazuji. Původně jsem chtěl kamarádsky varovat uživatele, že jestliže do tohoto algoritmu zadá 1000 prvků v poli, tak si hodně dlouho počká (například do konce vesmíru). Nicméně jsem usoudil že je uživatel inteligentní a v případě, že by mu to už několik dní počítalo by proces zastavil
- Zastavovací tlačítko. Příšlo mi jako opravdu velmi šikovný nápad až do chvíle, kdy jsem zjistil, že celý proces běží na jednom vlákně, tedy něco bych musel přesunout na druhé vlákno.
---

## 7. Jak přidat ředící algoritmus

1. Naprogramujte si ho. V každém kroku musí vracet IEnumerable<Sortstep>
    1. Dělá se to pomocí yield return
    2. Smysluplný krok je porovnání nebo výměna prvků
    3. na konci řekněte že je řazení hotové, jinak nebude vidět poslední krok (opět yield return akorát do sortstep musíte přidat hodnotu Done)
2. Přidejte ho do seznamu algorithms na začátku souboru.
3. Dívejte se jak řadí

## 8. Poznámka pod čarou

Při práci na projektu jsem spolupracoval s umělou inteligencí. Všechen kód jsem však napsal vlastní rukou. S umělou inteligenci jsem konzultoval pouze technické detaily. Jestli by to měl být problém, tak jsem schopný doložit chaty s umělou inteligencí. Myslím si však, že jak strukturou, stylem, tak samotným provedením je vidět, že to psal člověk.