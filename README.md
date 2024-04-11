# Travelling_Application
A web application that allows travelers to book hotels, flights, rent cars and view possible entertainment in the countries they plan to visit.

The main entities are TravellingApplication, Accommodation, Car, Entertainment, Air Ticket, Seller, Admin, Traveller, Hotel, Hostel, Apartment.

Let's consider each entity individually.

Travelingapplication:
List<Accommodation> - a list of all types of accommodation (hotels, hostels and apartments) provided on this website.
List<Car> - a list of all cars provided on this website for rent.
List<Air Ticket> - a list of all kinds of air tickets presented on this website.
List<Entertainment> - a list of all kinds of entertainment presented on this site.
List<Admin> - a list of site admins.
List<Traveller> is a list of the site's clients, they are also travelers.
List<Seller> - a list of people who provide the services offered on the site, that is, various airlines/hotel owners/owners of car rental companies.

Accommodation:
List<Hotel> - a list of all hotels presented on the site.
List<Apartment> - a list of all apartments presented on the site.
List<Hostel> - a list of all hostels presented on the site.

Car:
Name is the model of the car.
City - the city where this car can be rented.
Cost - the cost of renting a car for 1 day.
Transmission is the type of transmission of the machine.
Rating - the rating of the car.
AirCondition - the presence of an air conditioner in the car.
Descriprion - a detailed description of the car.

Entertainment:
City - the city where this entertainment is located.
Name - the name of the entertainment.
Rating - entertainment rating.
Cost - the price of entertainment for 1 person.
Date - the date on which this entertainment is available.
Descriprion - a detailed description of the entertainment.
Address - the address where the entertainment is located.

Air Ticket:
Cost - the price of an air ticket for 1 person.
Date - the date on which this ticket is valid.
CityFrom - which city the flight is from.
flightNumber is the flight number.
CityTo is the city to which the plane is going.

Seller:
Name - the name of the person or company providing the service.
Email - the email address used to log in to your personal account.
Password - the password used to log in to the account.
List<Accomadation> - a list of housing facilities provided by this person/company.
List<Cars> - a list of rental cars provided by this person/company.
List<Entertainment> - a list of entertainment provided by this person/company.
List<Air Ticket> - a list of air tickets provided by this person/company.
List<Travelers> - a list of website customers who have purchased something from this person/company.
AddAccomodation() is a method that allows you to send a new housing object for moderation by the administrator.
addCar() is a method that allows you to send a new car rental object for moderation by the admin.
AddEntertainment() is a method that allows you to send a new entertainment object for moderation by the administrator.
Addair Ticket() is a method that allows you to send a new ticket for moderation by the administrator.
EditInfo() is a method that allows you to change any information of objects previously published on the site by this author.
DeleteObject() is a method that allows you to delete a previously published object.

Admin:
Name - the name of the admin.
Email - the admin's email address used to log in to the personal account.
Password - the password used to log in to the site under the name of the administrator.
List<Accommodation> - a list of housing objects that "came" to the administrator for moderation.
List<Car> - a list of rented cars that "came" to the admin for moderation.
List<Entertainment> - a list of entertainment that "came" to the admin for moderation.
List<Air Ticket> - a list of air tickets that "came" to the administrator for moderation.
ConfirmAddition() is a method that results in publishing the proposed object to the public.
RefuseAddition() is a method that results in the refusal to publish the proposed object. This object will not be publicly available.
DeleteObject() is a method that allows the administrator to delete absolutely any object previously published on the site.

Traveller:
Name - the name of the client (he is also a traveler).
Email - the client's email address used to log into the personal account.
PhoneNumber is the client's phone number.
Password - the password used by the client to log into the personal account.
Rating - rating of the client as a traveler.
List<Accommodation> PastReservation - a list of housing properties that the client has previously visited.
List<Accommodation> FavoriteAccomodation - a list of housing objects that the client has added to his Favorites.
AddToFavorite() is a method that allows the client to add any object to favorites.
BookNow() is a method that allows the customer to make a reservation right now.
DeleteFromFavorite() is a method that allows the client to delete an object from favorites.
CancelBook() - cancellation of a valid reservation.
PayNow() is a method that allows you to pay for your reservation right now.

Hotel:
Name - the name of the hotel.
Address - the address of the hotel.
Rating - the rating of the hotel.
Parking - the presence or absence of parking in the hotel.
SwimmingPool - the presence or absence of a swimming pool on the territory of the hotel.
WIFI - the presence or absence of WIFI on site.
Cost - the cost of staying at this hotel for 1 night.
City - the city where this hotel is located.
Description - a detailed description of the hotel.

Hostel:
Name - the name of the hostel.
Address - the address of the hostel.
Rating - rating of the hostel.
Parking - the presence or absence of parking in the hostel.
SwimmingPool - the presence or absence of a swimming pool on the territory of the hostel.
WIFI - the presence or absence of WIFI on the territory of the hostel.
Cost - the cost of staying in this hostel for 1 night.
City - the city where this hostel is located.
Description - a detailed description of the hostel.

Apartment:
Name - the name of the apartments.
Address - the address of the apartments.
Rating - rating of apartments.
Parking - the presence or absence of parking in the apartments.
SwimmingPool - the presence or absence of a swimming pool on the territory of the apartments.
WIFI - the presence or absence of WIFI on the territory of the apartments.
Cost - the cost of living in these apartments for 1 night.
City - the city where these apartments are located.
Description - a detailed description of the apartments.
