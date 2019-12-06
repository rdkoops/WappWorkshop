# Workshop Charts
In deze workshop zullen we charts implementeren in ASP.NET Core MVC Application door Chart.js te gebruiken: [https://www.chartjs.org/](https://www.chartjs.org/)


### Vereisten

 - Windows 
 - Visual Studio 2019 Preview 
 - .NET Core SDK 3.1
 - Javascript

### Stap 1 - Creëer een ASP.NET Core 3.1 MVC Project**

Open opdracht prompt (of gebruik de ingebouwde terminal van Visual Studio) en start met het creëren van een folder voor je applicatie.

    mkdir ChartsDemo
    cd ChartsDemo
    dotnet new razor

Open de ChartsDemo folder in Visual Studio 2019 Preview


### Stap 2 - Download de Chart.js javascript file & data voor het weer
Ga naar:  
https://cdn.jsdelivr.net/npm/chart.js  
Download de file en plaats deze in de wwwroot, js folder (creëer de folder js als je dit niet al hebt).

Maak een ```data``` folder aan in de wwwwroot folder. Hierna download de json data van de git repo en plaat het in dit folder locatie.  
Ga naar:  
https://github.com/Kiiwuu/WappWorkshop/blob/master/Charts/ChartsDemo/wwwroot/data/weather.json


### Stap 3 - Creëer de model Weatherforecast

Maak een folder aan in de project folder die heet ```Models```. 
Maak daar een een klas die heet ```WeatherForecast```.

Dit is de hoofd model, de entiteit met een lijst met ```weatherForecasts```.

Binnen de hoofdmethode voeg de volgende code toe. 
```
        public DateTime Date { get; set; }

        public string Day => Date.ToString("dddd");

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }        

```

Dit geeft een sort van template voor de data die wij nodig hebben. 

    
 ### Stap 4 - Creëer een Service die de data inlaad in Weatherforecast
We gaan nu een service maken. Dit laad het data van de JSON bestand in de vorm van onze model die wij hebben gemaakt in stap 3.

Maak een folder aan die het ```Services``` in de project folder.

Voeg hiernaa een nieuwe klas toe die heet ```JsonFileWeatherService```.

Binnen deze klas wordt er gebruik gemaakt van de ```weather.json``` bestand.  
*Tip: Hier zou je een database connectie kunnen maken en gebruik maken van een functie in de model om te zetten naar json.**


### Stap 5 - Creëer een controller voor routing
Maak een folder aan in de project map die heet ```Controllers```.

Rechter muis klik op de```Controllers``` map en ga naar ```Add > Controller```.

Kies voor ```API Controller - Empty``` en noem het controller ```WeatherForecastController```.


### Stap 6 - Verander de Startup 

Voeg de volgende code boven ```services.AddRazorPages()``` binnen de ```ConfigureServices``` functie.

```
    services.AddTransient<InvoiceService>();
```

#### Extra uitleg 

De ```services.AddTransient<>()``` functie zorgt ervoor dat er een nieuwe instantie van wat er tussen de <> staat geleverd wordt aan elke controller en service die hem aanroept. 


### Stap 7 - Code voor backend Index page
Voeg Razor Pages toe via folder Home > Add > New > Razor Page > bestandsnaam "Index"
(als er nog geen Index.cshtml.cs file is) 
Open de Index.cshtml.cs file en vervang alle code met de onderstaande code:

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartsDemo.Models;
using ChartsDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ChartsDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonFileWeatherService WeatherService;
        public List<Weatherforecast> WeatherforecastList { get; private set; }
        public IndexModel(ILogger<IndexModel> logger, JsonFileWeatherService weatherService)
        {
            _logger = logger;
            WeatherService = weatherService;
        }

        public void OnGet()
        {
            WeatherforecastList = WeatherService.GetWeatherforecasts().ToList<Weatherforecast>();
        }

        public JsonResult OnGetWeatherforecastChartData()
        {
            WeatherforecastList = WeatherService.GetWeatherforecasts().ToList<Weatherforecast>();
            Console.WriteLine($"LIJST: {WeatherforecastList}");
            var weatherChart = new CategoryChartModel();
            weatherChart.AmountList = new List<double>();
            weatherChart.CategoryList = new List<string>();

            foreach (var weather in WeatherforecastList)
            {
                weatherChart.AmountList.Add(weather.TemperatureC);
                weatherChart.CategoryList.Add(weather.Day);
            }

            return new JsonResult(weatherChart);
        }

    }
}
```

De ```OnGet``` method - laad de invoice list zodat deze kan worden weergegeven op de pagina.
loads the invoice list to be displayed in the page
De ```OnGetWeatherforecastChartData``` methode is de backend voor de ```fetch``` functie in de Javascript Code. Het zal de JSON data leveren zodat het kan worden weergegeven op de pagina.



### Stap 8 - Index page - Teken de charts door Javascript te gebruiken

#### Stap 8.1
Open de ```Index.cshtml``` file, en vervang de inhoudt van de file met het volgende.:
    @page
    @model ChartsDemo.Pages.IndexModel
    @{
        ViewData["Title"] = "Home page";
    }
    
    <div class="text-center">
        <h1 class="display-4">Welcome</h1>
        <p>Lets take a look at the weather!</p>
        <h1 class="display-4">This month's weather</h1>
    </div>

Dit zal een erg eenvoudige html pagina renderen met de heading "Welcome..."
De applicatie zou op dit punt moeten werken. Je kan de applicatie runnen door naar het **Debug** menu te gaan en **Start Without Debugging** of door **Ctrl-F5 **. De applicatie zou nou moeten openen in je default web browser
Het is belangrijk om de ```@page``` directive bovenaan de .cshtml te definieren. De @page directive verteld Razor dat deze .cshtml file een Razor Page representeert.
De ```@page``` directive wordt gevolgd door een ```@model``` directive. Dit identificeert de corresponderende C# model klasse , die gelokaliseerd is in dezelfde folder van de .cshtml pagina zelf.
Met een ```@{}``` blok wordt server-side code toegevoegd. 

#### Stap 8.2
We voegen Chart.js toe door de script tag toe te voegen aan onze razor page.

```<script src="~/js/Chart.bundle.min.js"></script>```

#### Stap 8.3

Hierna hebben we een canvas nodig voor onze pagina. 

``` 
<div class="container">
    <canvas id="weatherChart" width="500" height="300"></canvas>
</div> 
```

#### Stap 8.4

Nu kunnen we een chart creeeren. We voegen een script toe aan onze pagina. 

Om  chart te creeren, moeten we de ```Chart``` class instantieren. Om dit te doen, moeten we het canvas doorgeven in de node.
```let popCanvasName = document.getElementById("weatherChart");  ```

Als je het element hebt, kan je zelf een pre-gedefinieerd chart-type of een eigen instantieren.

Het onderstaande geeft alle code weer om een bar chart te instantieren. 

```
<script>
    // Data
    var myAmounts = [];
    var myCategories = [];
    var myWeather;

    function showChart() {
        myAmounts = myWeather.amountList;
        myCategories = myWeather.categoryList;
        console.log(myWeather);
        console.log(myAmounts);
        console.log(myCategories);
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
    