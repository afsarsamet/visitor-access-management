
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/aracHub")
    .build();

connection.on("AracAsimYapti", function (gelenId) {
    console.log(" Aşım yapan araç ID -> " + gelenId);

    let satir = document.getElementById("arac-satiri-" + gelenId);

    if (satir) {
        let durumSutunu = satir.querySelector(".durum-sutunu");

        if (durumSutunu) {
           
            durumSutunu.innerHTML = '<span class="badge rounded-pill" style="background-color: #f8d7da; color: #842029; border: 1px solid #f1aeb5; padding: 6px 12px;">Aşım var</span>';
        }
    }
});

connection.start().then(function () {
    console.log("SignalR Aktif.");
}).catch(function (err) {
    console.error("Başarısız ", err.toString());
});


function SureleriGuncelle() {
    let hucreler = document.querySelectorAll('.sayac-hucresi');
    let suAn = new Date();

    hucreler.forEach(function (hucre) {
        let girisZamani = new Date(hucre.getAttribute('data-giris'));
        let farkMilisaniye = suAn - girisZamani;

        let toplamDakika = Math.floor(farkMilisaniye / 1000 / 60);
        let saat = Math.floor(toplamDakika / 60);
        let dakika = toplamDakika % 60;

        hucre.innerText = saat + " saat " + dakika + " dk";

        if (toplamDakika > 240) {
            
            hucre.style.color = "#dc3545";
            hucre.style.backgroundColor = "transparent";
            hucre.style.fontWeight = "900";
        }
    });
}

window.onload = function () {
    SureleriGuncelle();
    setInterval(SureleriGuncelle, 60000);
};

