// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Site adlarını çeker ve listeye atar, 1. ad seçili olarak gelir
var adSelect = document.querySelector("#adSelect");
let ops = adSelect.options;
let adlar = [];
for (var i = 0; i < ops.length; i++) {
    adlar.push(ops[i].value);
}
var seciliAd = adlar[0];
//barkod ve ad girişlerinin değişken kaydetme fonksiyonları
function adSec() {
    var selectedValue = adSelect.options[adSelect.selectedIndex].value;
    seciliAd = selectedValue.toUpperCase();
    search();
}
var aranacakBarkod = "";
var barkodInput = document.querySelector("#barkodArama")
function barkodAra() {
    aranacakBarkod = barkodInput.value;
    search();
}
var tarihInput = document.querySelector("#tarihSelector");
var aranacakTarih = tarihInput.value;
function tarihSec() {
    aranacakTarih = tarihInput.value;
    search();
}

//tabloyu simpledatatables kütüphanesi tablosuna çevirir == hızlı ve takılmadan arama için
const myTable = document.querySelector("#productTable");
const dataTable = new simpleDatatables.DataTable(myTable, {
    sortable: false,
    perPage: 25
});
dataTable.on("datatable.init", search());
//kütüphane tablosunun arama kutusunu kaldırır (sıkıntı çıkartıyordu çoklu aramada)
document.querySelector(".datatable-top").style.display = "none";


//kaydedilen değişkenlerle arama fonksiyonu
function search() {
    dataTable.multiSearch([{ term: seciliAd, columns: [2] }, { term: aranacakBarkod, columns: [0] }, { term: aranacakTarih, columns: [7] }]);
    if (dataTable.totalPages==0) {
        dataTable.multiSearch([{ term: seciliAd, columns: [2] }, { term: aranacakBarkod, columns: [0] }, { term: "-", columns: [7] }]);
        var toast = document.createElement("div");
        toast.innerHTML = '<div aria-live="polite" aria-atomic="true" class="bg - dark position - relative bd - example - toasts">            < div class="toast-container position-absolute p-3" id = "toastPlacement" >                <div class="toast top-0 end-0">                    <div class="toast-header">                        <img src="..." class="rounded me-2" alt="...">                            <strong class="me-auto">Bootstrap</strong>                            <small>11 mins ago</small>                    </div>                    <div class="toast-body">                        Hello, world! This is a toast message.                    </div>                </div>  </div ></div >'
        document.body.appendChild(toast);
    }
}

