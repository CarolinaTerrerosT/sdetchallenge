# API Automation Challenge 
# I created two files, features and models, in features I put all the tests and their functionalities and models I put the model that I needed.
# Features: I created 3 test methods:
# 1. Test List_Station_ReturnsAllMeasurements: The goal of this method is return all measurement of oak street station, however this station didn't bring data and only show an empty list, I added other method "for each" to do an iteration, If You replace that with 63rd Street Weather Station you can see the data is working. I used client and request variables, to call the api and hit it, I created a content variable to execute the request and added weatherMeasurements to do the deserialization with the list.
# 2. Test ListMeasurements_ComparePages_DataDoesNotRepeat:The objective of this method is comparing pages with 10 limit per page, I created 2 request variables with their parameters each one, I added 2 content variables, also 2 variables of weathermeasurement and finally compared two pages with except method. 
# 3. Test Fail_MalformedQuery_ErrorMessage : For this method I created a request with a malformed query and we expected a 400 bad request error message, also I created an error model to receive the error object. Finally, I asserted the malformed compiler error and the could not parse Soql query error message.

# Notes:  I followed the api dev socrata documentation, soql and authentication token