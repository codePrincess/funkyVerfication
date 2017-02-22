[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)]() [![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)]()


# ID Verification made simple

The function implemented in this repo helps you verify the type of an identification card. Currently it works for German passports and national identification cards (Personalausweis) but it is easily extendable to other types of cards.

You can fire up your own Azure Function if you want to host the logic in your own environment or just use the following url for a first try. 

https://idverificationapp.azurewebsites.net/api/cardVerification

Just attach an image to the body and get a JSON response back with the info, what type your identification card is. 
Current types are PASSPORT, PERSO and UNKNOWN.

Keep in mind: 
- As soon as you want to host this service in your own function, you have to get a key for the Microsoft Cognitive Service Computer Vision APIs and enter it in the functions "appKey" variable.
If you don't have an account for Cognitive Services yet just visit https://www.microsoft.com/cognitive-services and create one for free.
- The usage of the Computer Vision API is quite low on the costs - and the first 5000 calls are completely free of charge - yey!

So have fun and get back to me if you encounter some issues :)


