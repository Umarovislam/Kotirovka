using DynamicData.Binding;
using Kotirovka.Extensions;
using Kotirovka.Model;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;

namespace Kotirovka.ViewModel
{
    public class ApplicationViewModel : ReactiveObject
    {
        private DataTable table;

        private Dictionary<string, Valute> _valutesList;
        private string _searchText;
        private decimal usd { get; set; }
        private List<string> _codes;

        private decimal _termResult;
        private ConvertResult convertResult;
        private decimal firstVal;
        private decimal secondVal;
        private string firstIndex;
        private string secondIndex;
        public decimal FirstTb
        {
            get => this.firstVal;
            set
            {
                this.RaiseAndSetIfChanged(ref firstVal, value);
                if (firstIndex != null && secondIndex != null )
                {
                    var f = ((_valutesList[firstIndex].Value * firstVal) / _valutesList[firstIndex].Nominal);
                    var d = ((_valutesList[secondIndex].Value) / _valutesList[secondIndex].Nominal);
                    SecondTb = decimal.Round(f / d, 2, MidpointRounding.AwayFromZero);
                }
            }
        }
        public decimal SecondTb
        {
            get => this.secondVal;
            set
            {
                this.RaiseAndSetIfChanged(ref secondVal, value);
                if (firstIndex != null && secondIndex != null )
                {
                    var f = ((_valutesList[secondIndex].Value * secondVal) / _valutesList[secondIndex].Nominal);
                    var d = ((_valutesList[firstIndex].Value) / _valutesList[firstIndex].Nominal);
               // FirstTb = decimal.Round(f / d, 2,MidpointRounding.AwayFromZero);
                }
            }
        }
        public string FirstIndex
        {
            get => firstIndex ?? firstIndex;
            set => this.RaiseAndSetIfChanged(ref firstIndex, value);
        }
        public string SecondIndex
        {
            get => secondIndex ?? secondIndex;
            set => this.RaiseAndSetIfChanged(ref secondIndex, value);
        }



        public Dictionary<string, Valute> ValutesList
        {
            get => _valutesList;
            set => this.RaiseAndSetIfChanged(ref this._valutesList, value);
        }
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref this._searchText, value);
        }
        public decimal TermResult
        {
            get
            {
                if(firstIndex != null && secondIndex != null)
                {
                    return (((_valutesList[firstIndex].Value * firstVal) / _valutesList[firstIndex].Nominal) /
                    ((_valutesList[secondIndex].Value * secondVal) / _valutesList[secondIndex].Nominal));
                }
                return 0;
            }
        }

        public DataTable GridTable
        {
            get => table;
            set => this.RaiseAndSetIfChanged(ref table, value);
        }

        public ConvertResult ConvertResult
        {
            get => convertResult ?? convertResult;
            set => this.RaiseAndSetIfChanged(ref convertResult, value);
        }
        public List<string> Codes
        {
            get => _codes ?? _codes;
            set { 
                this.RaiseAndSetIfChanged(ref _codes, value);
                this.WhenAnyPropertyChanged();
            }
        }
        public ReactiveCommand<Unit,Dictionary<string, Valute>> LoadAndRefreshCommand { get; }
        public ReactiveCommand<Unit,Unit> SearchCommand { get;  } 

        public ApplicationViewModel()
        {
           
            LoadAndRefreshCommand = ReactiveCommand.CreateFromObservable(LoadAndRefreshResult);
            SearchCommand = ReactiveCommand.CreateFromObservable(SearchTermResult);
        }

        public IObservable<Dictionary<string, Valute>> LoadAndRefreshResult()
        {
            Cbr Format;
            GridTable = TableExtension.ToDataTable();
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead("https://www.cbr-xml-daily.ru/daily_json.js");
                StreamReader reader = new StreamReader(stream);
                var Content = reader.ReadToEnd();
                Format = JsonConvert.DeserializeObject<Cbr>(Content);
            }
            this.ValutesList = Format.Valute;
            this.Codes = Format.Valute.Values.Select(p => p.CharCode).ToList();
            this.usd = Format.Valute.Values.FirstOrDefault(p => p.CharCode == "USD").Value;

            TableExtension.Insert(GridTable, Format.Valute.Values.ToList());
            return Observable.Return(Format.Valute);
        }
        public IObservable<Unit> SearchTermResult()
        {
            if(this._valutesList != null && !string.IsNullOrWhiteSpace(SearchText))
            {
                var val = ValutesList.Values.FirstOrDefault(p => p.CharCode == SearchText.ToUpper() || p.Name == SearchText);
                if(val != null)
                this.ConvertResult = new ConvertResult { Code = val.CharCode, Nominal = val.Nominal, Rubl = val.Value, Usd =decimal.Round(usd / val.Value,4,MidpointRounding.AwayFromZero) };
            }
            return Observable.Return(Unit.Default);
        }

    }
}
