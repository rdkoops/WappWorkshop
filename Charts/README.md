# Workshop Charts
In deze workshop zullen we charts implementeren in ASP.NET Core MVC Application door ... te gebruiken

### Vereisten

 - Windows 
 - Visual Studio 2019 Preview .NET Core
 - SDK 3.1
 - Javascript

### Stap 1 - Creëer een ASP.NET Core 3.1 MVC Project**

Open opdracht prompt (of gebruik de ingebouwde terminal van Visual Studio) en start met het creëren van een folder voor je applicatie.

    mkdir ChartsDemo
    cd ChartsDemo
    dotnet new razor

Open de ChartsDemo folder in Visual Studio 2019 Preview

### Stap 2 - Download de Chart.js javascript file
Ga naar https://cdn.jsdelivr.net/npm/chart.js
Download de file en plaats deze in de wwwroot, js folder.

### Stap 3 - Creeer de klas InvoiceModel

This is the main model, the entity for list of invoices. The second class there will be used to provide data to the Chart, is see point E below.

Ga naar tools > NuGet Package Manager > Manage NuGet Packages for Solution > Browse > Newtonsoft.JSON > Install

    using Newtonsoft.Json;
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
    
### Stap 5 - Index page - tekent de charts door Javascript te gebruiken

    @page
    @model ChartsDemo.Views.Home.IndexModel
    @{
    }
    
    @{
        ViewData["Title"] = "Home page";
    }
    
    <script src="~/js/Chart.js"></script>
    
    
    <div class="text-center">
        <h1 class="display-4">Invoice List</h1>
    </div>
    
    <table class="table table-sm">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(model => model.InvoiceList[0].InvoiceNumber)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.InvoiceList[0].Amount)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.InvoiceList[0].CostCategory)
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model.InvoiceList)
            {
                <tr>
                    <td>
                        @Html.DisplayFor(modelItem => item.InvoiceNumber)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.Amount)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.CostCategory)
                    </td>
                </tr>
            }
        </tbody>
    </table>
    
    <div class="container">
        <canvas id="invChart" width="500" height="300"></canvas>
    </div>
    
    <script>
    
        /////////
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

      public void ConfigureServices(IServiceCollection services)
            {
                // TOEGEVOEGD ----------
                services.AddTransient<InvoiceService>();
                //---------------------
                services.AddRazorPages();
            }
