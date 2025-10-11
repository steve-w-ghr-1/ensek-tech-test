Feature: Retrieve available energy products
    
    The API will allow all available energy product details that can be ordered
    
    # primary happy path implemented

    Background: 
        Given I have reset the test data   
        
    Scenario: Get energy products
        When I request to view all energy products
        Then the response status code is '200'
        And the response contains data as in file 'energyExpectedStaticData.json'    
