# Workshop Charts
In deze workshop zullen we charts implementeren in ASP.NET Core MVC Application door Chart.js te gebruiken: [https://www.chartjs.org/](https://www.chartjs.org/)


### Vereisten

 - Windows 
 - Visual Studio 2019 Preview 
 - .NET Core SDK 3.1
 - Javascript kennis

### Stap 1 - Creëer een ASP.NET Core 3.1 MVC Project

Open opdracht prompt (of gebruik de ingebouwde terminal van Visual Studio) en navigeer naar de gewenste map door middel van:

    cd [FOLDER PATH]

 Start hierna met het creëren van een nieuwe folder voor je applicatie.

    mkdir ChartsDemo
    cd ChartsDemo
    dotnet new razor

Open de ChartsDemo folder in Visual Studio 2019 Preview

### Stap 2 - Download de Chart.js javascript file & data voor het weer
Ga naar:  
https://cdn.jsdelivr.net/npm/chart.js@2.9.3/dist/Chart.min.js  
Download de file doormiddel van het klikken op de rechtermuisknop. 
**Zorg dat het bestand ```Chart.bundle.min.js``` heet!**
Dan op opslaan als (kies hierbij de gewenste locatie om op te slaan). Vervolgens plaats je dit bestand in de wwwroot van je project, in de js folder (creëer de folder js als je dit niet al hebt). 

Maak een ```data``` folder aan in de wwwwroot folder. Hierna download of clone je de repository van github. Als je de map hebt uitgepakt zie je in de hoofdmap een json file staan genaamd weather.json. Plaats deze in de data folder die je net aangemaakt hebt.  
Ga naar:  
https://github.com/Kiiwuu/WappWorkshop

In de Visual Studio Preview solution explorer ziet je wwwroot mar er dan als volgt uit:

![wwwroot map](https://github.com/Kiiwuu/WappWorkshop/blob/master/Charts/images/1.JPG)

### Stap 3 - Creëer het model Weatherforecast en het model CategoryChart

Maak een folder aan in de project folder die ```Models``` heet. 
Maak daar een een klas die heet ```WeatherForecast```.

Je structuur ziet er dan als volgt uit:

![wwwroot map](https://github.com/Kiiwuu/WappWorkshop/blob/master/Charts/images/2.JPG)

Dit is het hoofd model. Het is een entiteit met een lijst met ```weatherForecasts```.

Voeg binnen de class ```weatherForecasts``` onderstaande variabele toe: 
```csharp
        public DateTime Date { get; set; }

        public string Day => Date.ToString("dddd");

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }        

```

Dit geeft een template voor de data die wij nodig gaan hebben. 

Maak vervolgens in dezelfde map (```Models```) nog een class aan die ```CategoryChart``` heet. In deze class slaan wij de gegevens die wij willen weergeven in een chart op in een array. Voor nu weergeven wij alleen een chart van de tempratuur per dag. De class ```CategoryChart``` ziet er dan als volgt uit:

```csharp
public class CategoryChart
    {
        public List<string> CategoryList { get; set; }

        public List<double> AmountList { get; set; }

    }
```
    
 ### Stap 4 - Creëer een Service die de data inlaad in Weatherforecast
We gaan nu een service maken. Deze laad de data van het JSON bestand in de vorm van het model die wij hebben gemaakt in stap 3.

Maak een folder in het project aan die het ```Services``` heet.

Voeg hiernaa een nieuwe klas toe die heet ```JsonFileWeatherService```.

Je file structuur zou er als volgt uit moeten zien:

![wwwroot map](https://github.com/Kiiwuu/WappWorkshop/blob/master/Charts/images/3.JPG)

Binnen deze class wordt er gebruik gemaakt van het ```weather.json``` bestand.  

*Tip: Hier zou je een database connectie kunnen maken en gebruik kunnen maken van een functie om de data om te zetten in een json file.**

Voeg vervolgens onderstaande code toe:

```csharp
    public class JsonFileWeatherService
    {
        public JsonFileWeatherService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        public IWebHostEnvironment WebHostEnvironment { get; }
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "weather.json"); }
        }
        public IEnumerable<WeatherForecast> GetWeatherforecasts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<WeatherForecast[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }
    }
```

In deze functie wordt doormiddel van de ```JsonSerializer.Deserialize<WeatherForecast[]>``` het json bestand omgezet in een array van het ```WeatherForecast``` model. In de ```webHostEnvironment``` variabele zit de data opgeslagen die betrekking heeft op het web hosting process. 

Waarschijnlijk moet je eerst nog alles importeren. 
**Vergeet dit niet!**

### Stap 5 - Verander de Startup 

Ga naar ```Startup.cs```.

Voeg de volgende code boven ```services.AddRazorPages()``` binnen de ```ConfigureServices``` functie.

```csharp
    services.AddTransient<JsonFileWeatherService>();
```

De ```services.AddTransient<>()``` functie zorgt ervoor dat er een nieuwe instantie van wat er tussen de <> staat geleverd wordt aan elke controller en service die hem aanroept. 


### Stap 6 - Creëer een controller voor routing

Maak een folder aan in de project map die heet ```Controllers```.

Rechter muis klik op de map ```Controllers``` en ga naar ```Add > Controller``` (bovenste optie).

Kies voor ```API Controller - Empty``` en noem het controller ```WeatherForecastController```.

*Het kan zijn dat je een pop-up krijgt van waar je hem wilt opslaan. Het pad is als het goed is al ingevuld, je hoeft dan alleen op opslaan te klikken. Hierna krijg je waarschijnlijk een optie om het project te herladen. Kies hier voor de optie 'Reload'. Als je nu naar de map ```Controllers``` gaat zie je dat de controller is toegevoegd.*  

**Let op!** In dit demo hebben wij de  ```[Route("api/[controller]")]``` verandered naar ```[Route("[controller]")]```. Er is geen reden hiervoor dus als je het laat staan zoals het is vergeet het niet toe te passen bij de rest van de endpoints.

Voeg de volgende twee stukken code toe:
```
 [Route("[controller]")]
    [ApiController]
    public class WeatherforecastController : ControllerBase
    {
        public WeatherforecastController(JsonFileWeatherService weatherService)
        {
            this.WeatherService = weatherService;
        }

        public JsonFileWeatherService WeatherService { get; set; }

    }
```
Voeg nu een nieuwe route toe:
```
        [HttpGet]
        public IEnumerable<Weatherforecast> GetWeatherforecasts()
        {
            return WeatherService.GetWeatherforecasts();
        }
```

### Stap 7 - Code voor backend Index page

Ga naar de map ```Pages``` en druk op het pijltje naast de ```index.cshtml```. Er verschijnt nu de ```Index.cshtml.cs``` class, dit is de code achter de ```index.cshtml``` html pagina.   

Open de ```Index.cshtml.cs``` file en vervang alle code met de onderstaande code:

```csharp
    public class IndexModel : PageModel
    {
        public JsonFileWeatherService WeatherService;
        public List<WeatherForecast> WeatherforecastList { get; private set; }
        public IndexModel(JsonFileWeatherService weatherService)
        {
            WeatherService = weatherService;
        }

        public void OnGet()
        {
            WeatherforecastList = WeatherService.GetWeatherforecasts().ToList<WeatherForecast>();
        }

        public JsonResult OnGetWeatherforecastChartData()
        {
            WeatherforecastList = WeatherService.GetWeatherforecasts().ToList<WeatherForecast>();
            Console.WriteLine($"LIJST: {WeatherforecastList}");
            var weatherChart = new CategoryChart();
            weatherChart.AmountList = new List<double>();
            weatherChart.CategoryList = new List<string>();

            foreach (var weather in WeatherforecastList)
            {
                weatherChart.AmountList.Add(weather.TemperatureC);
                weatherChart.CategoryList.Add(weather.Day);
            }

            return new JsonResult(weatherChart);
        }
```

De ```OnGet``` method laad de Weatherforecast list zodat deze kan worden weergegeven op de pagina.
De ```OnGetWeatherforecastChartData``` methode is de backend voor de ```fetch``` functie in de Javascript Code. Het zal de JSON data leveren zodat het kan worden weergegeven op de pagina.

**Vergeet wederom niet de imports!**

### Stap 8 - Index page - Teken de charts door Javascript te gebruiken

#### Stap 8.1
Open de ```Index.cshtml``` file, en vervang de inhoud van de file met het volgende:

```csharp
    @page
    @model IndexModel
    @{
        ViewData["Title"] = "Home page";
    }
    
    <div class="text-center">
        <h1 class="display-4">Welcome</h1>
        <p>Lets take a look at the weather!</p>
        <h1 class="display-4">This month's weather</h1>
    </div>
```

Dit zal een erg eenvoudige html pagina renderen met de heading "Welcome"
De applicatie zou op dit punt moeten werken. Je kan de applicatie runnen door naar het **Debug** menu te gaan en **Start Without Debugging**, door **Ctrl-F5** te drukken of door gewoon op de **play knop** te drukken. De applicatie zou nou moeten openen in je default web browser.
Het is belangrijk om de ```@page``` directive bovenaan de .cshtml te zetten. De ```@page``` directive verteld Razor dat deze .cshtml file een Razor Page representeert.
De ```@page``` directive wordt gevolgd door een ```@model``` directive. Dit identificeert de corresponderende C# model class, die gelokaliseerd is in dezelfde folder van de .cshtml pagina zelf.
Met een ```@{}``` blok wordt server-side code toegevoegd. 

#### Stap 8.2
We voegen Chart.js toe door de script tag toe te voegen aan onze razor page.

```<script src="~/js/Chart.bundle.min.js"></script>```

Je page ziet er dan als volgt uit:

![wwwroot map](https://github.com/Kiiwuu/WappWorkshop/blob/master/Charts/images/4.JPG)

#### Stap 8.3

Hierna hebben we een canvas nodig voor onze pagina. 
Voeg onderstaande code onder de ```<div class="text-center">```.

```csharp 
<div class="container">
    <canvas id="weatherChart" width="500" height="300"></canvas>
</div> 
```

#### Stap 8.4

Nu kunnen we een chart creëren. We voegen een script toe aan onze pagina om de chart te weergeven met de goede data. 

Het onderstaande geeft alle code weer om een bar chart te instantieren. Voeg deze ```<script>``` tag onder de ```<div class="container">``` toe.

```csharp
<script>
    // Data
    var myAmounts = [];
    var myCategories = [];
    var myWeather;

    function showChart() {
        myAmounts = myWeather.amountList;
        myCategories = myWeather.categoryList;
        let popCanvasName = document.getElementById("weatherChart");
        let weatherChart = new Chart(popCanvasName, {
            type: 'bar',
            data: {
                labels: myCategories,
                datasets: [{
                    label: 'Weather data',
                    data: myAmounts,
                    backgroundColor: getRandomColorEachData(myCategories.length),
                }]
            },
            options: {
                responsive: false,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    }

    // Get data from API endpoint
    function getChartData() {
        return fetch('./Index?handler=WeatherforecastChartData',
            {
                method: 'get',
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8'
                }
            })
            .then(function (response) {
                if (response.ok) {
                    return response.text();
                } else {
                    throw Error('Response Not OK');
                }
            })
            .then(function (text) {
                try {
                    return JSON.parse(text);
                } catch (err) {
                    throw Error('Method Not Found');
                }
            })
            .then(function (responseJSON) {
                myWeather = responseJSON;
                showChart();
            })
    }

    // Color the chart
    var getRandomColor = function() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
    }

    function getRandomColorEachData(count) {
    var data =[];
    for (var i = 0; i < count; i++) {
        data.push(getRandomColor());
    }
    return data;
}
    getChartData();
</script>
```

Als je dit gedaan hebt kan je hem runnen door op de play knop te drukken.

Om de chart te creëren, moeten we de ```Chart``` class instantieren. Om dit te doen, moeten we het canvas doorgeven in de node. Dit gebeurt in onderstaande regel. (Deze hoef je niet meer op te nemen in je project)

```let popCanvasName = document.getElementById("weatherChart");  ```

Als je het element hebt, kan je zelf een pre-gedefinieerd chart-type of een eigen instantieren.
