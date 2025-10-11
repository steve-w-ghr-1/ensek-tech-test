Feature: Update energy orders
    
    The API will allow a specified energy order to be updated
    
    Background: 
        Given I have a valid API secure token
        And I have reset the test data
    
    # There's a number of observations/issues to report around the update operation.
    # These mean I have not implemented the tests. Some example scenarios 
    # (commented out and not implemented) have been provided which I would normally
    # suggest (if not listed in the story) to be implemnted when once any issue has been
    # understood / resolved.
        
    # In most cases would perform order updates against the existing loaded in test data. However,
    # there would be some tests we'd want to seed orders via the API and then perform operations on.
    # We'd also want to make sure that the static data is always maintained if the any affected changes to API
    # e.g. contract change.  
        
    Scenario Outline: Update an order
        When I request an update to an order with details:
          | Key       | Value      |
          | id        | <orderId>  |
          | quantity  | <qty>      |
          | energy_id | <energyId> |  
        Then the response status code is '200'
        And the response contains
          | Key       | Value      |
          | energy_id | <energyId> |
          | fuel      | <fuel>     |
          | id        | <orderId>  |
          | quantity  | <qty>      |
          | time      | [today]    |          
        
        Examples:
          | orderId                              | energyId | fuel | qty |
          | 080d9823-e874-4b5b-99ff-2021f2a59b24 | 4        | oil  | 19  |

    Scenario Outline: Update an order with a non existing order Id
        When I request an update to an order with details:
          | Key       | Value      |
          | id        | <orderId>  |
          | quantity  | <qty>      |
          | energy_id | <energyId> |  
        Then the response status code is '500'       
        
        Examples:
          | orderId | energyId | fuel | qty |
          | 999     | 4        | oil  | 19  |
          
# othe secanrios like check idempotency, include other invalid request data to test API validation