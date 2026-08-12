# Ziyaretçi Erişim Yönetim Sistemi

[🇬🇧 English README](README.md)

**ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server** ve **SignalR** kullanılarak geliştirilmiş web tabanlı bir ziyaretçi ve araç erişim yönetim sistemidir.

Uygulama; ziyaretçi araçlarının giriş ve çıkış işlemlerini yönetmek, tesiste bulunan araçları takip etmek, firma onay süreçlerini yürütmek, süre aşımlarını tespit etmek ve Excel raporları oluşturmak amacıyla geliştirilmiştir.

> İlk olarak endüstriyel bir tesiste ziyaretçi ve araç erişim yönetimi amacıyla, yazılım geliştirme stajı kapsamında geliştirilmiştir.

---

## Özellikler

* Ziyaretçi ve araç giriş kaydı
* Araç çıkış takibi
* Firma autocomplete araması
* Sistemde bulunmayan firmalar için onay süreci
* SignalR ile gerçek zamanlı süre aşımı bildirimi
* Arka planda araç süre kontrolü
* Cookie tabanlı kimlik doğrulama
* Güvenlik ve yönetici rolleri için yetkilendirme
* BCrypt ile güvenli parola hashleme
* Web Crypto API ile geçici parola üretimi
* Tarih ve aşım durumuna göre raporlama
* ClosedXML ile Excel raporu oluşturma
* Entity Framework Core ve SQL Server ile veri yönetimi

---

## Kullanılan Teknolojiler

| Teknoloji              | Kullanım Amacı                |
| ---------------------- | ----------------------------- |
| .NET 8                 | Uygulamanın çalışma platformu |
| ASP.NET Core MVC       | Backend web framework         |
| C#                     | Backend geliştirme dili       |
| Entity Framework Core  | ORM ve veritabanı erişimi     |
| SQL Server             | İlişkisel veritabanı          |
| SignalR                | Gerçek zamanlı iletişim       |
| Razor Views            | Sunucu taraflı dinamik arayüz |
| JavaScript / jQuery    | İstemci tarafı işlemler       |
| jQuery UI Autocomplete | Firma arama önerileri         |
| Bootstrap              | Responsive arayüz tasarımı    |
| BCrypt.Net             | Parola hashleme               |
| ClosedXML              | Excel raporu oluşturma        |
| Web Crypto API         | Güvenli geçici parola üretimi |

---

## Uygulama Mimarisi

Uygulama **MVC (Model-View-Controller)** mimarisini kullanır.

```text
Kullanıcı / Tarayıcı
      ↓ HTTP
Razor View + JavaScript
      ↓
Controller
      ↓
Entity Framework Core
      ↓
SQL Server
```

Gerçek zamanlı süre aşımı bildirimleri için kullanılan akış:

```text
Background Service
      ↓
Veritabanı Kontrolü
      ↓
SignalR Hub
      ↓
Bağlı Tarayıcılar
      ↓
Sayfa Yenilenmeden Arayüz Güncellemesi
```

---

## Kimlik Doğrulama ve Yetkilendirme

Uygulamada cookie tabanlı authentication kullanılmaktadır.

Başarılı bir giriş işleminden sonra:

1. Kullanıcı bilgileri doğrulanır.
2. Girilen parola BCrypt ile kayıtlı hash değeri üzerinden kontrol edilir.
3. Sicil numarası, ad-soyad ve rol bilgileri claim olarak oluşturulur.
4. ASP.NET Core korumalı bir authentication cookie üretir.
5. Sonraki HTTP isteklerinde authentication middleware kullanıcı kimliğini yeniden oluşturur.

Sistemde iki temel rol bulunmaktadır:

* **Güvenlik**
* **Admin**

Role-based authorization ile yönetici işlemlerine erişim sınırlandırılır.

---

## Araç Giriş ve Çıkış Yönetimi

Güvenlik personeli ziyaretçi aracı kaydederken şu bilgileri girebilir:

* Plaka
* Ziyaretçi adı soyadı
* Telefon numarası
* Kişi sayısı
* Ziyaret nedeni
* Firma
* Giriş zamanı

Giriş zamanı sunucu tarafından oluşturulur.

Araç tesisten ayrıldığında ayrıca:

* Çıkış zamanı
* Çıkışı veren güvenlik personelinin sicil numarası
* Çıkışı veren personelin ad-soyad bilgisi

kaydedilir.

Bu yapı, araç giriş ve çıkış işlemlerinin geriye dönük olarak takip edilmesini sağlar.

---

## Firma Autocomplete

Firma alanında AJAX tabanlı autocomplete sistemi kullanılmaktadır.

Kullanıcı firma adını yazmaya başladığında:

```text
Kullanıcı Girişi
   ↓
AJAX GET İsteği
   ↓
Firma Arama Endpoint'i
   ↓
Entity Framework Sorgusu
   ↓
SQL Server
   ↓
JSON Cevabı
   ↓
Autocomplete Önerileri
```

Yalnızca aktif firmalar kullanıcıya gösterilir.

Ziyaret kaydında firma adı doğrudan tekrar tekrar saklanmak yerine ilgili firmanın `FirmaId` değeri tutulur.

Kavramsal ilişki:

```text
VisitorLog.FirmaId
        ↓
Firma.FirmaId
```

Bu yapı ilişkisel veritabanı normalizasyonunu destekler ve firma isimlerinin farklı veya hatalı biçimlerde tekrar kaydedilmesini azaltır.

---

## Firma Onay Süreci

Autocomplete aramasında firma bulunamazsa güvenlik personeli yeni firma talebi oluşturabilir.

İş akışı:

```text
Güvenlik personeli firma adını bildirir
        ↓
Firma talebi kaydedilir
        ↓
Yönetici talebi inceler
        ↓
Talep onaylanırsa aktif firma oluşturulur
        ↓
Yeni firma autocomplete sonuçlarında görünür
```

Talebi oluşturan personelin sicil ve ad-soyad bilgileri kullanıcı tarafından değiştirilebilen form alanlarından değil, authentication claim'lerinden alınır.

---

## Gerçek Zamanlı Süre Aşımı Kontrolü

Uygulamada çalışan bir Background Service, tesiste bulunan araçları belirli aralıklarla kontrol eder.

Bir araç belirlenen süreden uzun süre içeride kalırsa:

```text
Background Service
       ↓
Araç Süre Kontrolü
       ↓
Süre Aşımı Tespit Edilir
       ↓
Veritabanı Güncellenir
       ↓
SignalR Olayı
       ↓
Bağlı Tarayıcı
       ↓
Araç Satırı Güncellenir
```

SignalR sayesinde istemcilerin sunucuya sürekli AJAX isteği göndermesine gerek kalmadan, sunucu değişiklik oluştuğunda açık tarayıcılara doğrudan bildirim gönderebilir.

---

## Geçici Parola Üretimi

Yeni güvenlik personeli oluşturulurken istemci tarafında geçici parola üretilir.

Parola üretiminde:

* Büyük harf
* Küçük harf
* Rakam
* Özel karakter
* `crypto.getRandomValues()`
* Fisher-Yates karıştırma algoritması

kullanılır.

`Math.random()` yerine Web Crypto API kullanılması, güvenlik amacıyla daha uygun rastgele değerler üretilmesini sağlar.

Oluşturulan açık parola HTTPS üzerinden backend'e gönderilir ve veritabanına kaydedilmeden önce BCrypt ile hashlenir.

Açık parolalar veritabanında saklanmaz.

---

## Raporlama

Araç kayıtları şu kriterlere göre filtrelenebilir:

* Başlangıç tarihi
* Bitiş tarihi
* Süre aşımı durumu

Entity Framework Core, seçilen filtrelere göre dinamik LINQ sorgusu oluşturur.

Raporlar **ClosedXML** kullanılarak `.xlsx` formatında indirilebilir.

Raporda aşağıdaki bilgiler bulunabilir:

* Plaka
* Ziyaretçi bilgileri
* Firma
* Giriş zamanı
* Çıkış zamanı
* Süre aşımı durumu
* Çıkışı gerçekleştiren güvenlik personeli

---

## Proje Yapısı

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
│
├── Hubs/
│   └── AracHub.cs
│
├── Migrations/
│
├── Models/
│   ├── dbContextClass.cs
│   ├── Firma.cs
│   ├── FirmaTalep.cs
│   ├── Personel.cs
│   └── visitorLog.cs
│
├── Services/
│   └── SureKontrolIscisi.cs
│
├── Views/
│
├── wwwroot/
│   └── js/
│       └── sayac.js
│
├── Program.cs
├── appsettings.json
└── WebApplication1.csproj
```

---

## Veritabanı Modelleri

### Personel

Sisteme giriş yapabilen güvenlik ve yönetici kullanıcılarını temsil eder.

Başlıca bilgiler:

* Sicil numarası
* Ad soyad
* Rol
* Hesap durumu
* Parola hash değeri

### Firma

Araç giriş işlemlerinde seçilebilen aktif veya pasif firmaları temsil eder.

### FirmaTalep

Sistemde bulunmayan firmalar için oluşturulan ve yönetici onayı bekleyen talepleri temsil eder.

### visitorLog

Araçların giriş, çıkış ve ziyaret bilgilerini temsil eder.

Bir firma birden fazla ziyaret kaydıyla ilişkilendirilebilir:

```text
Bir Firma
    ↓
Birden Fazla Ziyaret Kaydı
```

---

## Kurulum

### Gereksinimler

Aşağıdaki araçların kurulu olması gerekir:

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 veya Visual Studio Code
* Git

---

### Repository'yi Klonlama

```bash
git clone <REPOSITORY_URL>
cd WebApplication1
```

---

### Bağımlılıkları Yükleme

```bash
dotnet restore
```

---

### Veritabanı Bağlantısını Yapılandırma

Hassas yapılandırma bilgilerinin doğrudan repository içerisinde tutulmaması önerilir.

Yerel geliştirme ortamında .NET User Secrets kullanılabilir:

```bash
dotnet user-secrets init
```

SQL Server bağlantısını tanımlayın:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=SUNUCU_ADI;Database=VisitorAccessDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

Başlangıç yönetici parolası gerekiyorsa:

```bash
dotnet user-secrets set "InitialAdminPassword" "GUVENLI_PAROLANIZ"
```

---

### Veritabanı Migration'larını Uygulama

Entity Framework CLI kurulu değilse:

```bash
dotnet tool install --global dotnet-ef
```

Migration'ları uygulayın:

```bash
dotnet ef database update
```

---

### Uygulamayı Çalıştırma

```bash
dotnet run
```

Terminalde gösterilen HTTPS adresini tarayıcıda açın.

---

## Güvenlik Yaklaşımı

Projede kullanılan bazı güvenlik mekanizmaları:

* BCrypt parola hashleme
* Cookie tabanlı authentication
* Role-based authorization
* Claims tabanlı kullanıcı bilgileri
* Sunucu tarafında oluşturulan giriş zamanları
* Hassas yerel ayarlar için User Secrets
* Entity Framework tarafından kullanılan parametreli SQL sorguları

Proje geliştirildikçe aşağıdaki alanlar daha da geliştirilebilir:

* Veri değiştiren işlemlerde anti-forgery doğrulaması
* ViewModel tabanlı sunucu tarafı validasyon
* Giriş işlemlerinde rate limiting
* İlk girişte parola değiştirme zorunluluğu
* Audit log sistemi
* UTC tabanlı zaman yönetimi
* Veritabanında unique constraint'ler
* SignalR endpoint'lerinde ek yetkilendirme

---

## Gelecekte Eklenebilecek Özellikler

* Unit ve integration testleri
* GitHub Actions CI pipeline
* Docker desteği
* Audit log sistemi
* Hesap kilitleme ve parola sıfırlama
* Dashboard ve istatistik ekranları
* Sayfalama ve gelişmiş filtreleme
* ViewModel tabanlı gelişmiş validasyon
* E-posta veya SMS bildirimleri
* Merkezi hata yönetimi ve loglama
* Cloud ortamına deployment

---

## Bu Projede Uygulanan Konular

Bu proje kapsamında aşağıdaki konularda pratik yapılmıştır:

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* LINQ
* Dependency Injection
* HTTP request pipeline
* Middleware
* Cookie Authentication
* Claims
* Role-based Authorization
* Code First Migration
* Foreign Key
* Navigation Property
* AJAX
* JSON
* SignalR
* Background Service
* BCrypt
* Web Crypto API
* Fisher-Yates Shuffle
* Excel raporu oluşturma

---

## Proje Durumu

Uygulama ilk olarak staj kapsamında çalışan bir proje olarak geliştirilmiştir ve şu anda kişisel yazılım geliştirme portföy projesi olarak sürdürülmektedir.

İlerleyen süreçte kod düzenleme, test, güvenlik geliştirmeleri ve deployment çalışmaları eklenebilir.
