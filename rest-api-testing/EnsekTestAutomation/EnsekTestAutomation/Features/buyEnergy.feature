Feature: Buy energy product
    
    The API will allow availble energy product to be ordered.
    
    Only implemented a test covering the key task requirements. I will comment out some example sceanrios
    that would be implemented to give a quick rough idea of how I would test the API, but wont be exhaustive.

    Background: 
        Given I have a valid API secure token
        And I have reset the test data
    
    # note: 2 tests in the following secanrio fail correctly on the 4th step. The casing on the order response of 'id' is also 'Id'.
    # we could make the extraction of the orderId case insentive on the key but that would be masking an issue    
        
    Scenario Outline: Each energy product with units available is ordered for MINIMUM quantity
        When I order '<qty>' units of '<energyType>' energy
        Then the response status code is '200'
        And the energy purchase response contains my order number
        And I see my energy order in the current order list
        #popping the below test step here for now to meet the task criteria.
        And I see '5' orders placed before today
        
        Examples:
            | qty | energyType |
            | 1   | gas        |
            | 1   | oil        |
            | 1   | electric   |
            
    Scenario Outline: Buy an energy product with 0 available units
        When I order '<qty>' units of '<energyType>' energy
        Then the response status code is '200'
        And the response contains
          | Key     | Value                                 |
          | message | There is no nuclear fuel to purchase! |
        
        Examples:
          | qty | energyType |
          | 1   | nuclear    |
            
    Scenario: I request a non existing energyType
        When I order '1' units of 'fake' energy
        Then the response status code is '400'
            
# As per previous messaging, unimplemented test specifications.

#    Scenario Outline: Each energy product is ordered with units available for MAXIMUM available quantity
#        Given I know how many energy units are available for each energy type
#        When I order '<qty>' units of '<energyType>' energy
#        Then the response status code is '200'
#        And the energy purchase response contains my order number
#        And I see my energy order in the current order list
#        And there is no remaining units for energy type <energyType>
        
#        Examples:
#          | qty   | energyType |
#          | [max] | gas        |
#          | [max] | oil        |
#          | [max] | electric   |
#          #| [max] | nuclear    |

#    Scenario Outline: Each energy product ordered EXCEEDS available quantity
#        Given I know how many energy units are available for each energy type
#        When I order '<qty>' units of '<energyType>' energy
#        Then ? - requirements to be defined, would expected to return a 400 with appropriate messaging
    
#        Examples:
#          | qty       | energyType |
#          | [exceeds] | gas        |
#          | [exceeds] | oil        |
#          | [exceeds] | electric   |
#          | [exceeds] | nuclear    |