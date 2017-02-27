[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)]()


# ID Verification made simple

The function implemented in this repo helps you verify the type of an identification card. Currently it works for German passports and national identification cards (Personalausweis) but it is easily extendable to other types of cards.

<img src="https://dl.dropboxusercontent.com/u/2095227/blogscribbels/idverification.jpg" alt="Identification card verification" />

You can fire up your own Azure Function if you want to host the logic in your own environment or just use the following url for a first try. 

https://idverificationapp.azurewebsites.net/api/cardVerification

## How to use it:
1. Attach an image to the body of POST request to the URL https://idverificationapp.azurewebsites.net/api/cardVerification
2. Get a JSON based response with the info enclosed, what type your identification card is

## Response
A JSON dictionary with the return code (HTTP OK if everything went fine) and an ID value
```
enum id_type {
    case PASSPORT
    case PERSO
    case UNKNOWN
}
```
## Test images
The prototype can be tested with those two images of a German passport and an national ID card
<img src=https://upload.wikimedia.org/wikipedia/commons/9/9e/Mustermann_Reisepass_2007.jpg width=300 />
<img src=https://upload.wikimedia.org/wikipedia/commons/7/7e/Muster_des_Personalausweises_VS.jpg width=325/>

## Keep in mind
- As soon as you want to host this service in your own function, you have to get a key for the Microsoft Cognitive Service Computer Vision APIs and enter it in the functions "appKey" variable.
If you don't have an account for Cognitive Services yet just visit https://www.microsoft.com/cognitive-services and create one for free.
- The usage of the Computer Vision API is quite low on the costs - and the first 5000 calls are completely free of charge - yey!

# Disclaimer
This is just a first version to illustrate the idea for verification card validation based on the coordinates of text boxes. To make the service work reliable with different cards further checks are needed. Stay tuned!

So have fun and get back to me if you encounter some issues :)


