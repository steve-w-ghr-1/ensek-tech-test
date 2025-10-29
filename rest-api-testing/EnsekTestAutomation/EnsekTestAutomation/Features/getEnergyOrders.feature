Feature: Retrieve energy orders
    
    The API will allow all energy order details to be retrieved
    
    Background: 
        Given I have reset the test data   
        
    Scenario: serialise response to model when requesting example
        When I make the call and serialise to model example
        Then the following order data should be returned:
          | fuel     | id                                   | quantity | time                          |
          | electric | 080d9823-e874-4b5b-99ff-2021f2a59b25 | 23       | Mon, 7 Feb 2022 00:01:24 GMT  |
          | oil      | 080d9823-e874-4b5b-99ff-2021f2a59b24 | 25       | Thu, 10 Mar 2022 00:01:24 GMT |
          | gas      | 31fc32da-bccb-44ab-9352-4f43fc44ed4b | 5        | Thu, 10 Mar 2022 09:01:24 GMT |
          | nuclear  | 2cdd6f69-95df-437e-b4d3-e772472db8de | 15       | Tue, 08 Feb 2022 01:01:24 GMT |
          | gas      | 31fc32da-bccb-44ab-9352-4f43fc44ed4b | 5        | Thu, 10 Mar 2022 09:23:24 GMT |
        Then the order data is as shown in filename 'orderDetailsExpectedData.json' - model example
        