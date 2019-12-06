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

### Stap 2 - Download de Chart.js javascript file
Ga naar https://cdn.jsdelivr.net/npm/chart.js.

Download de file en plaats deze in de ```wwwroot```, js folder (creeër de folder js als je dit niet al hebt).

### Stap 3 - Creeer de model Weatherforecast

Maak een folder aan in de project folder die heet ```Models```. 
Maak daar een een klas die heet ```Weatherforecast```.

Dit is de hoofdklasse, de entiteit met een lijst met ```weatherforecasts```.
De tweede klasse ```CategoryChartModel``` zal worden gebruikt om data te leveren aan de Chart.

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    namespace ChartsDemo.Models
    {
        public class InvoiceModel
        {
            public int InvoiceNumber { get; set; }
            public double Amount { get; set; }
            public string CostCategory { get; set; }
    
        }
    
        public class CategoryChartModel
        {
            [JsonProperty(PropertyName = "CategoryList")]
            public List<string> CategoryList { get; set; }
    
            [JsonProperty(PropertyName = "AmountList")]
            public List<double> AmountList { get; set; }
    
        }
    }
    
 ### Stap 4 - Creeer een Service die de data inlaad in InvoiceModel
 We laden hier handmatig data in een list.
 
    using ChartsDemo.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    namespace ChartsDemo.Services
    {
        public class InvoiceService
        {
            public List<InvoiceModel> GetInvoices()
            {
                return new List<InvoiceModel>()
                {
                    new InvoiceModel() {InvoiceNumber = 1, Amount = 10, CostCategory = "Utilities"},
                    new InvoiceModel() {InvoiceNumber = 2, Amount = 50, CostCategory = "Telephone"},
                    new InvoiceModel() {InvoiceNumber = 3, Amount = 30, CostCategory = "Services"},
                    new InvoiceModel() {InvoiceNumber = 4, Amount = 40, CostCategory = "Consultancy"},
                    new InvoiceModel() {InvoiceNumber = 5, Amount = 60, CostCategory = "Raw materials"}
                };
            }
        }
    }
    
### Stap 6 - Verander de Startup 
Voeg AddTransient toe. 

```
// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
            services.AddTransient<JsonFileWeatherService>();
        }

```

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