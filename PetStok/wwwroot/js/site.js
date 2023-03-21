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
    seciliAd = selectedValue;
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
document.querySelector("#indexBas").addEventListener("click", indexBas);
function indexBas() {
    var requestOptions = {
        method: 'GET',
        redirect: 'follow'
    };
    fetch("http://193.53.103.155:8090/api/Genel/indexBas", requestOptions)
        .then(response => response.text())
        .then(basariliToast())
        .catch(error => console.log('error', error));
}
function basariliToast() {
    setTimeout(function () {
        var toast = document.querySelector("#indexToast");
        toast.classList.remove("hide");
        toast.classList.add("show");
        toast.classList.remove("show");
        toast.classList.add("hide");
    }, 5000);
}
var icerik = document.querySelector("#style-div");
var yukleniyor = document.querySelector("#yukleniyor");

//tabloyu simpledatatables kütüphanesi tablosuna çevirir == hızlı ve takılmadan arama için
const myTable = document.querySelector("#productTable");
const dataTable = new simpleDatatables.DataTable(myTable, {
    sortable: false,
    perPage: 25
});
dataTable.on("datatable.init", function () {
    yukleniyor.style.display = "none";
    search();
});
//kütüphane tablosunun arama kutusunu kaldırır (sıkıntı çıkartıyordu çoklu aramada)
document.querySelector(".datatable-top").style.display = "none";


//kaydedilen değişkenlerle arama fonksiyonu
function search() {
    dataTable.multiSearch([{ term: seciliAd, columns: [2] }, /*{ term: aranacakBarkod, columns: [0] },*/ { term: aranacakTarih, columns: [7] }]);
    if (dataTable.totalPages == 0) {
        var toast = document.querySelector("#liveToast");
        toast.classList.remove("hide");
        toast.classList.add("show");
        dataTable.multiSearch([{ term: seciliAd, columns: [2] }, /*{ term: aranacakBarkod, columns: [0] },*/ { term: "-", columns: [7] }]);
        setTimeout(function () {
            toast.classList.remove("show");
            toast.classList.add("hide");
        }, 5000);
    }
}
