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
    dotnet new mvc

Open de ChartsDemo folder in Visual Studio 2019 Preview

### Stap 2 - Download de Chart.js javascript file
Ga naar tools > NuGet Package Manager > Manage NuGet Packages for Solution > Browse > Chart.js > Install

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
