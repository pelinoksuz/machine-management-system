# Machine Management System – Temel Endpoint Kataloğu (.NET)

Bu doküman, yeni bir **Machine Management System** projesi için ilk iterasyonda hazırlanması gereken temel REST endpointlerini özetler.

## 1) Sistem / Operasyonel Endpointler

- `GET /api/health/live`  
  Uygulamanın ayakta olup olmadığını kontrol eder (liveness).
- `GET /api/health/ready`  
  Uygulamanın istek almaya hazır olup olmadığını kontrol eder (readiness: DB, queue, cache).
- `GET /api/version`  
  API versiyonu, build bilgisi ve deploy zamanı döner.

## 2) Kimlik Doğrulama ve Yetkilendirme

- `POST /api/auth/login`  
  Kullanıcı giriş yapar, access/refresh token döner.
- `POST /api/auth/refresh`  
  Refresh token ile yeni access token üretir.
- `POST /api/auth/logout`  
  Refresh token iptali / oturum kapatma.
- `GET /api/auth/me`  
  Giriş yapan kullanıcının profil ve rol bilgileri.

## 3) Kullanıcı ve Rol Yönetimi (RBAC)

- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`
- `GET /api/roles`
- `POST /api/roles`
- `PUT /api/roles/{id}`
- `DELETE /api/roles/{id}`

> Not: Silme işlemleri için soft-delete yaklaşımı önerilir.

## 4) Makine Yönetimi (Ana Domain)

- `GET /api/machines`  
  Filtreleme/sıralama/paging desteklemeli (status, siteId, model, criticality vb.).
- `GET /api/machines/{id}`
- `POST /api/machines`
- `PUT /api/machines/{id}`
- `PATCH /api/machines/{id}/status`  
  Çalışıyor/Duruş/Bakımda/Hurda vb. statü güncelleme.
- `DELETE /api/machines/{id}`

## 5) Makine Bileşenleri ve Konfigürasyon

- `GET /api/machines/{id}/components`
- `POST /api/machines/{id}/components`
- `PUT /api/machines/{id}/components/{componentId}`
- `DELETE /api/machines/{id}/components/{componentId}`
- `GET /api/machines/{id}/configuration`
- `PUT /api/machines/{id}/configuration`

## 6) Bakım Yönetimi

- `GET /api/maintenance-plans`
- `POST /api/maintenance-plans`
- `PUT /api/maintenance-plans/{id}`
- `GET /api/work-orders`
- `POST /api/work-orders`
- `PUT /api/work-orders/{id}`
- `PATCH /api/work-orders/{id}/status`

## 7) Arıza / Olay Kaydı

- `GET /api/incidents`
- `GET /api/incidents/{id}`
- `POST /api/incidents`
- `PUT /api/incidents/{id}`
- `PATCH /api/incidents/{id}/status`

## 8) Sensör Verisi ve Telemetri

- `POST /api/telemetry`  
  Cihaz veya edge agent veri gönderimi.
- `GET /api/machines/{id}/telemetry`
- `GET /api/machines/{id}/metrics`  
  Özet metrikler (uptime, MTBF, enerji tüketimi vb.).

## 9) Alarm / Bildirim

- `GET /api/alerts`
- `POST /api/alerts`
- `PATCH /api/alerts/{id}/acknowledge`
- `PATCH /api/alerts/{id}/resolve`

## 10) Doküman / Dosya Yönetimi

- `POST /api/files`  
  Kılavuz, bakım raporu, fotoğraf gibi dosya yükleme.
- `GET /api/files/{id}`
- `DELETE /api/files/{id}`
- `GET /api/machines/{id}/files`

## 11) Audit / Log

- `GET /api/audit-logs`  
  Kim, neyi, ne zaman değiştirdi?
- `GET /api/machines/{id}/history`  
  Makine zaman çizelgesi (durum değişimi, bakım, arıza, parça değişimi).

## 12) Dashboard / Raporlama

- `GET /api/dashboard/overview`
- `GET /api/reports/availability`
- `GET /api/reports/failures`
- `GET /api/reports/maintenance-cost`

## API Tasarımında Temel Kurallar

- Tüm liste endpointlerinde: `page`, `pageSize`, `sortBy`, `sortOrder`, `search` desteği.
- Hata formatı için tek bir standart sözleşme (ProblemDetails / RFC7807).
- Kimliği kritik endpointlerde idempotency anahtarı (özellikle `POST` iş emirleri).
- `v1` ile versiyonlama (`/api/v1/...`) baştan uygulanmalı.
- OpenAPI/Swagger zorunlu tutulmalı.

## İlk Sprint İçin MVP Önerisi

İlk sprintte aşağıdaki endpointler gerçeklenirse temel ürün akışı çalışır:

1. Auth: `login`, `refresh`, `me`
2. Machines: listele, detay, oluştur, güncelle, statü güncelle
3. Work Orders: listele, oluştur, statü güncelle
4. Incidents: listele, oluştur, statü güncelle
5. Health + Version endpointleri
