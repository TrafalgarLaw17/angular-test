##############################################################
# HAPPY PATHS
##############################################################

Feature: Create and Manage To-Do List
  As a software developer
  I want to create, retrieve, update, and delete tasks
  So that I can manage my development work efficiently

@happy
# Scenario 1: Successfully create a To-Do List
Scenario: Successfully create a To-Do List
  Given I create a To-Do List with valid title "Fix Login Bug" 
  And with valid description "Investigate and fix login issue"
  And the valid item status is set to default "Pending"
  When I send a POST request with valid data to Create To Do API "api/todo"
  Then the http status code response with valid data for creating an item should be 201
  And the http status response with valid data for creating an item should be SUCCESS
  And the http message response with valid data for creating an item should be "ITEM CREATED SUCCESSFULLY."

@happy
# Scenario 2: Successfully retrieve all To-Do items
Scenario: Successfully retrieve all To-Do items
  Given I have atleast one record of a To-Do item
  When I send a GET request with valid data to Retrieve All To Do API "api/todo"
  Then the http status code response with valid data for retrieving all items should be 200
  And the http status response with valid data for retrieving all items should be SUCCESS
  And the http message response with valid data for retrieving for all items should be "Retrieved items."

@happy
# Scenario 3: Successfully retrieve a To-Do item by Id
Scenario: Successfully retrieve a To-Do item by Id
  Given I have a To-Do item with valid id "1"
  When I send a GET request with valid data to Retrieve To Do Item by Id API "api/todo/1"
  Then the http status code response with valid data for retrieving the item should be 200
  And the http status response with valid data for retrieving the item should be SUCCESS
  And the http message response with valid data for retrieving the item should be "Retrieved item."

@happy
# Scenario 4: Successfully update a To-Do item by Id
Scenario: Successfully update a To-Do item by Id
  Given I want to update a record with valid id "1"
  And I want to update the valid title to "Fix Registration Bug"
  And I want to update the description with valid data to "Investigate and fix registration issue"
  When I send a PUT request with valid data to Update To Do Item by Id API "api/todo/1"
  Then the http status code response with valid data for updating the item should be 200
  And the http status response with valid data for updating the item should be SUCCESS
  And the http message response with valid data for updating the item should be "ITEM UPDATED SUCCESSFULLY."

@happy
# Scenario 5: Successfully delete a To-Do item by Id
Scenario: Successfully delete a To-Do item by Id
  Given I want to delete a record with valid id "1"
  When I send a DELETE request with valid data to Delete To Do Item by Id API "api/todo/1"
  Then the http status code response with valid data for deleting the item should be 200
  And the http status response with valid data for deleting the item should be SUCCESS
  And the http message response with valid data for deleting the item should be "ITEM DELETED SUCCESSFULLY."


##############################################################
# NEGATIVE PATHS
##############################################################

@negative
# Scenario 1: Failed to create a To-Do item with invalid data
Scenario: Failed to create a To-Do List
  Given I create a To-Do List with title ""
  And with description "Investigate and fix login issue"
  And with item status is set to default "Pending"
  When I send a POST request to Create To Do API "api/todo"
  Then the http status code response for creating an item should be 400
  And the http status response for creating an item should be FAIL
  And the http message response for creating an item should be "Not found or Invalid item data."

@negative
# Scenario 2: Failed to retrieve all To-Do items
Scenario: Failed to retrieve all To-Do items
  Given I have no records of To-Do items
  When I send a GET request to Retrieve All To Do API "api/todo"
  Then the http status code response for retrieving all items should be 500
  And the http status response for retrieving all items should be FAIL
  And the http message response for retrieving all items should be "An error occurred while retrieving items."

@negative
# Scenario 3: Failed to retrieve a To-Do item by Id with invalid data
Scenario: Failed to retrieve a To-Do item by Id with invalid data
  Given I have no To-Do item with id "999"
  When I send a GET request to Retrieve To Do Item by Id API "api/todo/999"
  Then the http status code response for retrieving the item should be 404
  And the http status response for retrieving the item should be FAIL
  And the http message response for retrieving the item should be "Not found or Invalid item data."

@negative
# Scenario 4: Failed to update a To-Do item by Id with invalid data
Scenario: Failed to update a To-Do item by Id with invalid data
  Given I want to update a record with id "999"
  And I want to update the title to "Invalid Title"
  And I want to update the description to "Valid Description"
  When I send a PUT request to Update To Do Item by Id API "api/todo/999"
  Then the http status code response for updating the item should be 404
  And the http status response for updating the item should be FAIL
  And the http message response for updating the item should be "Not found or Invalid item data."

@negative
# Scenario 5: Failed to delete a To-Do item by Id
Scenario: Failed to delete a To-Do item by Id
  Given I want to delete a record with id "999"
  When I send a DELETE request to Delete To Do Item by Id API "api/todo/999"
  Then the http status code response for deleting the item should be 404
  And the http status response for updating the item should be FAIL
  And the http message response for updating the item should be "Not found or Invalid item data."

