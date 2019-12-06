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
Ga naar https://cdn.jsdelivr.net/npm/chart.js
Download de file en plaats deze in de wwwroot, js folder (creeër de folder js als je dit niet al hebt).

### Stap 3 - Creeer de model Weatherforecast

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

    
 ### Stap 4 - Creeer een Service die de data inlaad in Weatherforecast
 We gaan nu een service maken. Dit laad het data van de JSON bestand in de vorm van onze model die wij hebben gemaakt in stap 3.

Maak een folder aan die het ```Services``` in de project folder.

Voeg hiernaa een nieuwe klas toe die heet ```JsonFileWeatherService```.

Binnen deze klas wordt er gemaakt van 
    
    
### Stap 5 - Index page - tekent de charts door Javascript te gebruiken

```
    <div class="container">
        <canvas id="invChart" width="500" height="300"></canvas>
    </div>
```

```    
    <script>
        var myAmounts = [];
        var myCategories = [];
        var myInvoices;
    
        function showChart() {
            myAmounts = myInvoices.AmountList;
            myCategories = myInvoices.CategoryList;
            console.log(myAmounts);
            console.log(myCategories);
            let popCanvasName = document.getElementById("invChart");
            let barChartName = new Chart(popCanvasName, {
              type: 'bar',
              data: {
                labels: myCategories,
                  datasets: [{
                      label: 'Invoice data',
                      data: myAmounts,
                      backgroundColor: [
                          'rgba(255, 99, 132, 0.6)',
                          'rgba(54, 162, 235, 0.6)',
                          'rgba(255, 206, 86, 0.6)',
                          'rgba(75, 192, 192, 0.6)',
                          'rgba(153, 102, 255, 0.6)',
                      ]
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
        function getChartData() {
            return fetch('./Index?handler=InvoiceChartData',
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
                    myInvoices = responseJSON;
                    showChart();
                })
        }
        getChartData();
    </script>
```


### Stap 6 - Code voor backend Index page
OnGet method - loads the invoice list to be displayed in the page
OnGetInvoiceChartData method - is the backend for the fetch at point D in the JavaScript Code. It will provide JSON data in order to be displayed with the list.

Voeg Razor Pages toe via folder Home > Add > New > Razor Page > bestandsnaam "Index"

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChartsDemo.Models;
    using ChartsDemo.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    
    namespace ChartsDemo.Views.Home
    {
        public class IndexModel : PageModel
        {
            private readonly InvoiceService _invoiceService;
    
            public List<InvoiceModel> InvoiceList;
            public IndexModel(InvoiceService invoiceService)
            {
                _invoiceService = invoiceService;
            }
            public void OnGet()
            {
                InvoiceList = _invoiceService.GetInvoices();
            }
    
            public JsonResult OnGetInvoiceChartData()
            {
                InvoiceList = _invoiceService.GetInvoices();
                var invoiceChart = new CategoryChartModel();
                invoiceChart.AmountList = new List<double>();
                invoiceChart.CategoryList = new List<string>();
    
                foreach (var inv in InvoiceList)
                {
                    invoiceChart.AmountList.Add(inv.Amount);
                    invoiceChart.CategoryList.Add(inv.CostCategory);
                }
    
                return new JsonResult(invoiceChart);
            }
    
        }
    }
    
### Stap 7 - Verander de Startup 
Voeg AddTransient toe. 

Voeg de volgende code boven ```services.AddRazorPages();``` binnen de ```ConfigureServices``` functie.

```
    services.AddTransient<InvoiceService>();
```
