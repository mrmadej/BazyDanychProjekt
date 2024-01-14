# BazyDanychProjekt

Aby projekt poprawnie działał, należy (np. w środowisku Visual Studio) sklonować repozytorium. Następnie środowisko może wymagać instalacji odpowiedniej wersji `.NET Core`. Potem należy zbudować projekt aby zainstalowały się wszystkie wymagane zależności.

Domyślnie dodany jest jeden użytkownik z uprawnieniami administratora.
LOGIN: admin
HASŁO: Pasword1!
Aby zalogować się jako zwykły użytkownik należy najpierw się zarejestrować

Po wejściu na stronę wyświetli się panel logowania i rejestracji. Po zalogwaniu zwykły użytkownik może przeglądać hotele, rezerwować pokoje w danym przedziale czasowym, dodawać opinie do hoteli oraz przeglądać swoje rezerwacje. Administrator posiada możliwość dodawania. usuwania i edytowania hoteli.

Baza danych składa się z następujących tabel:
hotel
opinia
pokoj
rezerwacja
uzytkownik
zdjecia

Ponadto użyte są tabele AspNet przechowujące role użytkowników.

opinie są połączone z tabelami:
uzytkownik
hotel

pokoj jest połączony z tabelą:
hotel

rezerwacje są połączone z tabelami:
uzytkownik
pokoj

zdjecie jest połączone z tabelą:
hotel
