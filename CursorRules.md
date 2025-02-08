# Kod Yazım ve Düzenleme Kuralları

## 1. Dosya Organizasyonu

- Her özellik (feature) kendi klasöründe bulunmalı
- Commands ve Queries ayrı klasörlerde tutulmalı
- Her Command/Query için Request, Response ve Handler dosyaları oluşturulmalı

## 2. Naming Conventions

- Interface'ler "I" prefix'i ile başlamalı (IRepository, IService)
- Abstract sınıflar "Base" prefix'i ile başlamalı (BaseEntity)
- Command/Query sınıfları ilgili suffix'leri içermeli (CreateBlogCommandRequest)
- Repository sınıfları Read/Write suffix'lerini içermeli (ProductReadRepository)

## 3. CQRS Yapısı

- Command ve Query ayrımı net yapılmalı
- Her Command/Query tek bir işi yapmalı
- Handler'lar IRequestHandler interface'ini implement etmeli
- Response objeleri standart yapıda olmalı (Status, Message, Data pattern'i)

## 4. Servis Katmanı

- Servisler interface üzerinden inject edilmeli
- Her servis tek bir sorumluluğa sahip olmalı
- Servis registrationları ilgili extension method'larda yapılmalı

## 5. Repository Pattern

- Generic repository pattern kullanılmalı
- Read/Write repository'ler ayrılmalı
- Repository'ler DbContext'e direkt erişmemeli

## 6. Error Handling

- Tüm metodlar try-catch bloğu içermeli
- Hatalar standart response formatında dönülmeli
- Custom exception'lar kullanılmalı

## 7. Validation

- FluentValidation kullanılmalı
- Validation'lar ayrı sınıflarda tutulmalı
- Cross-cutting validation'lar middleware ile yapılmalı

## 8. Dependency Injection

- Constructor injection kullanılmalı
- Scoped/Singleton/Transient lifecycle'ları doğru belirlenmeli
- Circular dependency'lerden kaçınılmalı

## 9. Code Style

- Metod isimleri PascalCase
- Değişken isimleri camelCase
- Interface property'leri PascalCase
- Yorum satırları Türkçe yazılabilir
