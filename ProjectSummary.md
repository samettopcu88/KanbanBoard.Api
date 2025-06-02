# Proje Özeti: Kanban Board API



## Proje Hakkında:

Bu proje, görevlerin listeler arasında sürüklenip bırakılabildiği bir görev yönetim uygulamasının temelini oluşturan bir Kanban Board sisteminin backend (API) kısmını geliştirmeyi hedeflemektedir. Projede;

- Kullanıcıların yeni board’lar oluşturabilmesi,

- Bu board’lara ait task listeleri (Backlog, To Do, In Progress, Done gibi),

- Ve her listeye ait kartlar oluşturup bu kartlar üzerinde çeşitli işlemler yapabilmesi beklenmektedir.



## Kullanılan Teknolojiler:

.NET 8 Web API

Entity Framework Core

AutoMapper

FluentValidation

SQL Server

Swagger

Postman



## İzlediğim Adımlar:

Proje Yapısını Oluşturma

- Projeyi ASP.NET Core Web API şablonu ile başlattım ve gerekli NuGet paketlerini yükledim.

- Katmanlı bir yapı kurdum: Controllers, Data(DbContext), Services, Repositories, Entities, Dtos, Mapping, Validators gibi klasörlerle kodları ayrıştırdım.

2. Veri Modelleme ve EF Core Ayarları

- Board, TaskList, Card adında üç temel entity tanımladım.

- İlişkileri Entity Framework Core üzerinden yapılandırdım.

- AppDbContext ile veri tabanı bağlantısını yönettim ve ilk migration'ı oluşturdum.

3. CRUD Operasyonları

- Board ve Card için gerekli tüm CRUD operasyonlarını yazdım.

- Her servis katmanını, repository’lerle iletişim kuracak şekilde inşa ettim.

- Özellikle Card için, liste taşıma (drag-drop mantığı) ve sıralama gibi özel mantıklar da eklendi.

4. DTO ve AutoMapper Kullanımı

- Entity sınıflarını dışarıya açmak yerine DTO sınıfları ile veri aktarımını sağladım.

- AutoMapper ile DTO-Entity dönüşümlerini merkezi bir profile üzerinden tanımladım.

5. Validation (Doğrulama)

- FluentValidation kütüphanesi ile, Board ve Card oluşturma işlemleri için giriş validasyonları tanımladım.

- Örneğin: BoardPublicId'nin benzersizliği, belirli karakter sınırları gibi.

6. PublicId Sistemini Özelleştirme

- Başlangıçta BoardPublicId sistem tarafından üretiliyordu.

- Daha sonra bu ID’yi kullanıcının kendisinin tanımlayabilmesi sağlandı.

- Bu işlem sırasında PublicId’nin eşsiz ve belirli formatta olması zorunlu kılındı.

7. Kod Kalitesi ve Yorumlar

- Karmaşık iş mantığı içeren bölümlere açıklayıcı yorum satırları ekledim.

- Projeyi okunabilir ve anlaşılır kılmaya dikkat ettim.

8. Postman Collection

- Geliştirdiğim tüm endpointleri bir Postman Collection haline getirdim.

- JSON formatında export edip projeye dahil ettim.

Test ve Son Kontroller:

- Tüm endpoint'ler manuel olarak Postman üzerinden test edildi.

- Validasyonlar ve hata mesajları cevapları kontrol edildi.

- Gereksiz kodlar temizlendi, yorumlar eklendi.



## Notlar:

- BoardPublicId, kullanıcı tarafından özelleştirilebilir ve benzersizliği kontrol edilir.

- Board oluştrulurken otomatik olarak 4 görev listesi (Backlog, To Do, In Progress, Done) eklenir.

- Card'lar otomatik olarak "Backlog" listesine düşecek şekilde ayarlandı.

- Kullanıcıların BoardPublicId ile işlem yapılması sağlandı.



## Sonuç:

Projede istenen tüm temel işlevsellikler tamamlanmış, kodlar temiz bir yapıda sunulmuş ve dokümantasyon ile desteklenmiştir.