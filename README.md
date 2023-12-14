<p>Aplikacja jest stworzona w architekturze .NET przy użyciu technologii Blazor, Rest API do obsługi żądań wysyłanych przez front oraz SQL Servera do przechowywania i przetwarzania danych.
Frontend został stworzony z wykorzystaniem biblioteki MudBlazor,
która udostępnia prosty i przejrzysty layout oraz opcje okienek dialogowych czy snackbary.</p>
<p>Front wykorzystuje funkcje liczące RRSO, stałą kwotę raty czy cały harmonogram spłat rat uwzględniając domyślne oprocentowanie prowizji oraz odsetek. Po potwierdzeniu pożyczki zostaje zamknięte okienko z suwakami,
a okienko z formularzem zostaje aktywowane.</p>

<p>Dostępne są trzy zakładki:
<ul>
  <li>w pierwszej podawane są podstawowe informacje o kliencie oraz odpowiednio walidowane,</li>
  <li>w drugiej podawane są kwoty miesięcznego dochodu i wydatków,</li>
  <li>a w trzeciej podawany jest numer rachunku, na który zostanie przelana pożyczka.</li>
</ul>
</p>

![index](https://github.com/EddyVuitton/MoneyLoaner/assets/76602435/db4e1f68-54f2-440c-a793-c7e60176964b)

---
<p>Po złożeniu wniosku sprawdzane jest czy klient jest już zarejestrowany w bazie, jeżeli nie to otwierane jest okienko do rejestracji konta oraz uzupełniane są pola wg tego co klient podał w formularzu.
W innym wypadku otwiera się okienko do zalogowania się. Można również zarejestrować się bez składania wniosku.</p>

![po_zlozeniu_wniosku](https://github.com/EddyVuitton/MoneyLoaner/assets/76602435/eaed4d19-09ee-494a-b550-e374eb955705)

---
<p>Backend przetwarza wniosek na podstawie tego co klient podał w formularzu.</p>
<p>Został ku temu stworzony scoring, który sprawdza czy różnica pomiędzy miesięcznym dochodem a wydatkami nie przekracza stałej kwoty raty oraz czy klient nie ma już jednej aktywnej, czyli nie spłaconej jeszcze pożyczki.</p>

![pozyczka_wyplacona](https://github.com/EddyVuitton/MoneyLoaner/assets/76602435/65c255c8-5bbc-4694-87e4-86c1606a7b99)

---

<p>Na stronie klienta dostępna jest opcja zmiany hasła, aktualnego adresu email czy numeru telefonu.</p>
