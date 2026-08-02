# Kardemir Ziyaretçi Araç Takip Sistemi

Kardemir bünyesinde gerçekleştirilen zorunlu yaz stajı kapsamında geliştirilen bu proje, tesise giriş yapan ziyaretçi araçlarının kayıt altına alınmasını, içeride kalma sürelerinin takip edilmesini, çıkış işlemlerinin yönetilmesini ve geçmiş kayıtların raporlanmasını sağlayan web tabanlı bir uygulamadır.

Uygulama; **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server** ve **SignalR** kullanılarak geliştirilmiştir.

## Projenin Amacı

Sistemin temel amacı, güvenlik personelinin ziyaretçi araç giriş-çıkış işlemlerini merkezi ve düzenli biçimde yönetebilmesini sağlamaktır.

Uygulama ile:

* Ziyaretçi ve araç bilgileri kaydedilebilir.
* Araçların ziyaret ettiği firma autocomplete alanıyla seçilebilir.
* Sistemde bulunmayan firmalar yönetici onayına gönderilebilir.
* Tesiste bulunan araçlar anlık olarak görüntülenebilir.
* Araçların giriş ve çıkış zamanları kaydedilebilir.
* Dört saatten uzun süre içeride kalan araçlar tespit edilebilir.
* Süre aşımı bilgisi SignalR ile açık ekranlara gerçek zamanlı iletilebilir.
* Tarih ve aşım durumuna göre rapor oluşturulabilir.
* Raporlar Excel formatında indirilebilir.
* Yönetici tarafından yeni güvenlik personeli oluşturulabilir.

## Kullanılan Teknolojiler

| Teknoloji              | Kullanım amacı                          |
| ---------------------- | --------------------------------------- |
| .NET 8                 | Uygulamanın çalışma platformu           |
| ASP.NET Core MVC       | Sunucu tarafı web mimarisi              |
| C#                     | Backend geliştirme dili                 |
| Entity Framework Core  | Veritabanı işlemleri ve ORM             |
| SQL Server             | Kalıcı veri depolama                    |
| SignalR                | Gerçek zamanlı süre aşımı bildirimi     |
| Razor View             | Dinamik HTML ekranları                  |
| JavaScript / jQuery    | İstemci tarafı işlemler ve AJAX         |
| jQuery UI Autocomplete | Firma arama ve öneri sistemi            |
| Bootstrap              | Arayüz düzeni ve responsive tasarım     |
| BCrypt.Net             | Parolaların güvenli biçimde hashlenmesi |
| ClosedXML              | Excel raporu oluşturulması              |

## Temel Özellikler

### Kimlik Doğrulama ve Yetkilendirme

* Cookie tabanlı kullanıcı oturumu
* Güvenlik ve Admin rolleri
* BCrypt ile parola doğrulama
* Role göre Controller erişim kontrolü
* Kullanıcı bilgilerinin claim yapısıyla taşınması

### Araç Giriş ve Çıkış Yönetimi

* Plaka, sürücü, telefon, kişi sayısı ve ziyaret nedeni kaydı
* Firma ile foreign key ilişkisi
* Giriş zamanının sunucu tarafından oluşturulması
* Çıkışı gerçekleştiren personelin sicil ve ad-soyad bilgisinin kaydedilmesi
* Tesiste bulunan araçların ayrı listelenmesi

### Firma Autocomplete Sistemi

Kullanıcının yazdığı firma adı AJAX ile sunucuya gönderilir. Aktif firmalar SQL Server üzerinde aranır ve sonuçlar JSON formatında istemciye döndürülür.

Kullanıcıya firma adı gösterilirken forma gerçek kayıt değeri olarak `FirmaId` yazılır:

```text
Görünen değer: KARDEMİR A.Ş.
Kaydedilen değer: FirmaId = 5
```

Bu yapı, firma adlarının tekrar tekrar yazılmasını ve yazım farklılıklarından kaynaklanan veri tutarsızlıklarını önler.

### Yeni Firma Talep Sistemi

Firma autocomplete sonuçlarında bulunamazsa kullanıcı yeni firma talebi oluşturabilir.

Talep akışı:

```text
Güvenlik personeli firma adını bildirir
        ↓
FirmaTalepleri tablosuna kayıt eklenir
        ↓
Yönetici talebi inceler
        ↓
Talep onaylanırsa Firmalar tablosuna eklenir
        ↓
Firma autocomplete sonuçlarında görünür
```

### Süre Kontrolü ve SignalR

`SureKontrolIscisi`, uygulama çalıştığı sürece arka planda belirli aralıklarla tesiste bulunan araçları kontrol eder.

Dört saati aşan bir araç bulunduğunda:

1. Araç kaydının aşım durumu güncellenir.
2. `IHubContext<AracHub>` üzerinden SignalR mesajı gönderilir.
3. Açık olan araç listesi ekranındaki ilgili satır sayfa yenilenmeden güncellenir.

### Personel ve Geçici Parola Oluşturma

Yeni personel oluşturulurken tarayıcı tarafında Web Crypto API kullanılarak rastgele geçici parola üretilir.

Parola üretiminde:

* Büyük harf
* Küçük harf
* Rakam
* Özel karakter
* `crypto.getRandomValues()`
* Fisher-Yates karıştırma algoritması

kullanılır.

Üretilen açık parola sunucuya ulaştıktan sonra BCrypt ile hashlenir. Veritabanında açık parola saklanmaz.

### Raporlama

Araç kayıtları:

* Başlangıç tarihi
* Bitiş tarihi
* Süre aşımı durumu

alanlarına göre filtrelenebilir. Sonuçlar ClosedXML kullanılarak `.xlsx` formatında indirilebilir.

## Proje Mimarisi

Uygulama MVC mimarisini kullanır:

```text
Kullanıcı / Tarayıcı
        ↓ HTTP
View — Razor, HTML, JavaScript
        ↓
Controller — İstek ve iş akışı yönetimi
        ↓
Entity Framework Core — LINQ ve veri erişimi
        ↓
SQL Server
```

Gerçek zamanlı bildirim akışı:

```text
SureKontrolIscisi
        ↓
IHubContext<AracHub>
        ↓
SignalR bağlantısı
        ↓
wwwroot/js/sayac.js
        ↓
Araç listesi ekranı
```

## Klasör Yapısı

```text
WebApplication1/
├── Controllers/
│   ├── FirmaController.cs
│   ├── GirisController.cs
│   ├── PersonelController.cs
│   ├── ReportsController.cs
│   ├── visitorLogsController.cs
│   ├── YoneticiAuthController.cs
│   └── YoneticiController.cs
├── Hubs/
│   └── AracHub.cs
├── Migrations/
├── Models/
│   ├── dbContextClass.cs
│   ├── Firma.cs
│   ├── FirmaTalep.cs
│   ├── Personel.cs
│   └── visitorLog.cs
├── Services/
│   └── SureKontrolIscisi.cs
├── Views/
├── wwwroot/
│   └── js/sayac.js
├── Program.cs
├── appsettings.json
└── WebApplication1.csproj
```

## Veritabanı Modelleri

### `Personel`

Sisteme giriş yapabilen güvenlik ve yönetici kullanıcılarını temsil eder.

### `Firma`

Araçların ziyaret ettiği aktif veya pasif firmaları temsil eder.

### `FirmaTalep`

Sistemde bulunmayan firmalar için oluşturulan yönetici onay taleplerini temsil eder.

### `visitorLog`

Araçların giriş, çıkış ve ziyaret bilgilerini temsil eder.

`visitorLog` ile `Firma` arasında bire-çok ilişki vardır:

```text
Bir firma → Birçok ziyaretçi araç kaydı
Bir araç kaydı → Bir firma
```

## Kurulum

### Gereksinimler

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 veya Visual Studio Code
* Git

### 1. Repository’yi Klonlayın

```bash
git clone <REPOSITORY_URL>
cd WebApplication1
```

### 2. NuGet Paketlerini Yükleyin

```bash
dotnet restore
```

### 3. Connection String Tanımlayın

Geliştirme ortamında bağlantı bilgisini kaynak kodda tutmak yerine .NET User Secrets kullanılması önerilir.

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=SUNUCU_ADI;Database=KDProject;Trusted_Connection=True;TrustServerCertificate=True;"
```

SQL Server kullanıcı adı ve parola ile kullanılacaksa connection string buna göre düzenlenmelidir.

### 4. Veritabanını Oluşturun

Entity Framework CLI yüklü değilse:

```bash
dotnet tool install --global dotnet-ef
```

Migration’ları veritabanına uygulayın:

```bash
dotnet ef database update
```

### 5. Uygulamayı Çalıştırın

```bash
dotnet run
```

Terminalde gösterilen HTTPS adresini tarayıcıda açın.

## Yapılandırma ve Güvenlik Notları

Bu proje bir staj çalışmasıdır. Canlı ortamda kullanılmadan önce aşağıdaki iyileştirmeler yapılmalıdır:

* Varsayılan yönetici parolası kaynak koddan kaldırılmalıdır.
* Parolalar ve connection string User Secrets veya secret manager ile yönetilmelidir.
* İlk girişte parola değiştirme zorunluluğu eklenmelidir.
* Giriş denemelerine rate limiting uygulanmalıdır.
* Tüm veri değiştiren POST endpoint’lerine anti-forgery doğrulaması eklenmelidir.
* `ReportsController` ve `AracHub` uygun rollerle yetkilendirilmelidir.
* Araç oluşturma işleminde entity yerine ViewModel kullanılmalıdır.
* `FirmaId` sunucu tarafında aktif ve geçerli firma olarak doğrulanmalıdır.
* Sicil numarası, T.C. kimlik numarası ve firma adı için gerekli unique index’ler eklenmelidir.
* Firma silme davranışı geçmiş araç kayıtlarını koruyacak biçimde düzenlenmelidir.
* Tarihler UTC olarak saklanmalıdır.
* Dört saat aşımı çıkış işlemi sırasında da kesin olarak hesaplanmalıdır.

> **Uyarı:** Gerçek şirket, personel, ziyaretçi veya bağlantı bilgileri herkese açık bir GitHub repository’sine yüklenmemelidir. Repository mümkünse private tutulmalıdır.

## Geliştirilebilecek Özellikler

* İlk girişte parola değiştirme ekranı
* Personel hesap kilitleme ve parola sıfırlama
* Firma ve personel yönetim panelleri
* Dashboard ve grafiksel istatistikler
* SignalR bağlantı grupları
* E-posta veya SMS bildirimleri
* Audit log sistemi
* Unit ve integration testleri
* Docker desteği
* Merkezi hata yönetimi ve loglama
* Sayfalama, sıralama ve gelişmiş arama

## Teknik Kazanımlar

Bu proje kapsamında aşağıdaki konular uygulanmıştır:

* ASP.NET Core MVC mimarisi
* HTTP request pipeline ve middleware yapısı
* Dependency Injection
* Cookie Authentication
* Role-based Authorization
* Claims yapısı
* Entity Framework Core ve LINQ
* Code First Migration
* Foreign key ve navigation property
* AJAX ve JSON iletişimi
* SignalR ile gerçek zamanlı iletişim
* Background Service
* BCrypt parola hashleme
* Web Crypto API
* Fisher-Yates algoritması
* Excel raporu oluşturma

## Proje Durumu

Proje staj çalışması kapsamında geliştirilmiş çalışan bir prototiptir. Üretim ortamına alınmadan önce güvenlik, doğrulama, test ve deployment süreçlerinin geliştirilmesi gerekmektedir.


