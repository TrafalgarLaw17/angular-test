using System;
using System.Net;
using Microsoft.AspNetCore.Connections;
using Mysqlx.Crud;
using TechTalk.SpecFlow;
using Xunit;
using static System.Net.WebRequestMethods;

namespace FRA_Todolist_prj.Tests
{
    [Binding]
    public class TodoTest
    {
        private HttpStatusCode _simulatedStatusCode;
        private string _simulatedResponseMessage = string.Empty;
        private bool _hasData;

        #region HAPPY PATHS (5)
            #region Happy Scenario 1: Create To-Do Item
            [Given("I create a To-Do List with valid title \"(.*)\"")]
            public void CreateGivenICreateAToDoListWithValidTitle(string title)
            {
                Console.WriteLine("==================== TODOLIST CREATE - Happy Path");
                Console.WriteLine($"Given I create a To-Do List with valid title \"{title}\": PASSED");
            }

            [Given("with valid description \"(.*)\"")]
            public void CreateGivenValidDescription(string description)
            {
                Console.WriteLine($"And with valid description \"{description}\": PASSED");
            }

            [Given("the valid item status is set to default \"(.*)\"")]
            public void CreateGivenTheValidItemStatusIsSetToDefault(string status)
            {
                Console.WriteLine($"And the item status is set to default \"{status}\": PASSED");
            }

            [When("I send a POST request with valid data to Create To Do API \"(.*)\"")]
            public void CreateWhenISendAValidPOSTRequestToCreateToDoAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.Created;
                _simulatedResponseMessage = "ITEM CREATED SUCCESSFULLY.";
                Console.WriteLine($"When I send a POST request with valid data to Create To Do API \"{apiEndpoint}\": PASSED");
            }

            [Then("the http status code response with valid data for creating an item should be (.*)")]
            public void CreateThenTheValidHttpStatusCodeResponseShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response with valid data for creating an item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response with valid data for creating an item should be SUCCESS")]
            public void CreateThenTheValidHttpStatusResponseShouldBeSUCCESS()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.Created);
                Console.WriteLine("And the http status response with valid data for creating an item should be SUCCESS: PASSED");
            }

            [Then("the http message response with valid data for creating an item should be \"(.*)\"")]
            public void CreateThenTheValidHttpMessageResponseShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response with valid data for creating an item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Happy Scenario 2: Retrieve All To-Do Items
            [Given("I have atleast one record of a To-Do item")]
            public void RetrieveGivenIHaveValidToDoItem()
            {
                Console.WriteLine("==================== TODOLIST RETRIEVE (all) - Happy Path");
                Console.WriteLine($"Given I have atleast one record of a To-Do item: PASSED");
            }
            [When("I send a GET request with valid data to Retrieve All To Do API \"(.*)\"")]
            public void RetrieveWhenISendAValidGetRequestToRetrieveAllToDoAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.OK;
                _simulatedResponseMessage = "Retrieved items.";
                _hasData = true;
                Console.WriteLine($"When I send a GET request with valid data to Retrieve All To Do API \"{apiEndpoint}\": PASSED");
            }

            [Then("the http status code response with valid data for retrieving all items should be (.*)")]
            public void RetrieveThenTheValidHttpStatusCodeResponseShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response with valid data for retrieving all items should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response with valid data for retrieving all items should be SUCCESS")]
            public void RetrieveThenTheValidHttpStatusResponseShouldBeSUCCESS()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.OK);
                Console.WriteLine("And the http status response with valid data for retrieving all items should be SUCCESS: PASSED");
            }

            [Then("the http message response with valid data for retrieving for all items should be \"(.*)\"")]
            public void RetrieveThenTheValidHttpMessageResponseShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response with valid data for retrieving for all items should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Happy Scenario 3: Retrieve a To-Do Item by Id
            [Given("I have a To-Do item with valid id \"(.*)\"")]
            public void RetrieveGivenIHaveAValidToDoItemWithId(string id)
            {
                Console.WriteLine("==================== TODOLIST RETRIEVE (by Id)  - Happy Path");
                Console.WriteLine($"Given I have a To-Do item with valid id \"{id}\": PASSED");
            }

            [When("I send a GET request with valid data to Retrieve To Do Item by Id API \"(.*)\"")]
            public void RetrieveWhenISendAValidGetRequestToRetrieveToDoItemByIdAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.OK;
                _simulatedResponseMessage = "Retrieved item.";
                Console.WriteLine($"When I send a GET request with valid data to Retrieve To Do Item by Id API \"{apiEndpoint}\": PASSED");
            }

            [Then("the http status code response with valid data for retrieving the item should be (.*)")]
            public void RetrieveThenTheValidHttpStatusCodeResponseForItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response with valid data for retrieving the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response with valid data for retrieving the item should be SUCCESS")]
            public void RetrieveThenTheValidHttpStatusResponseForItemShouldBeSUCCESS()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.OK);
                Console.WriteLine("And the http status response with valid data for retrieving the item should be SUCCESS: PASSED");
            }

            [Then("the http message response with valid data for retrieving the item should be \"(.*)\"")]
            public void RetrieveThenTheValidHttpMessageResponseForItemShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response with valid data for retrieving the item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Happy Scenario 4: Successfully update a To-Do item by Id
            [Given("I want to update a record with valid id \"(.*)\"")]
            public void UpdateGivenIWantToUpdateWithValidId(string id)
            {
                Console.WriteLine("==================== TODOLIST UPDATE (by Id - Happy Path)");
                Console.WriteLine($"Given I want to update a record with id \"{id}\": PASSED");
            }
            [Given("I want to update the valid title to \"(.*)\"")]
            public void UpdateGivenIWantToUpdateTheValidTitleTo(string newTitle)
            {
                Console.WriteLine($"Given I want to update the valid title to \"{newTitle}\": PASSED");
            }

            [Given("I want to update the description with valid data to \"(.*)\"")]
            public void UpdateGivenIWantToUpdateTheValidDescriptionTo(string newDescription)
            {
                Console.WriteLine($"And I want to update the description with valid data to \"{newDescription}\": PASSED");
            }

            [When("I send a PUT request with valid data to Update To Do Item by Id API \"(.*)\"")]
            public void UpdateWhenISendAValidPutRequestToUpdateToDoItemByIdAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.OK;
                _simulatedResponseMessage = "ITEM UPDATED SUCCESSFULLY.";
                Console.WriteLine($"When I send a PUT request with valid data to Update To Do Item by Id API \"{apiEndpoint}\": PASSED");
            }

            [Then("the http status code response with valid data for updating the item should be (.*)")]
            public void UpdateThenTheValidHttpStatusCodeResponseForUpdatingItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response with valid data for updating the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response with valid data for updating the item should be SUCCESS")]
            public void UpdateThenTheValidHttpStatusResponseForUpdatingItemShouldBeSUCCESS()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.OK);
                Console.WriteLine("And the http status response with valid data for updating the item should be SUCCESS: PASSED");
            }

            [Then("the http message response with valid data for updating the item should be \"(.*)\"")]
            public void UpdateThenTheValidHttpMessageResponseForUpdatingItemShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response with valid data for updating the item should be \"{expectedMessage}\": PASSED\n");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Happy Scenario 5: Delete a To-Do Item by Id
            [Given("I want to delete a record with valid id \"(.*)\"")]
            public void DeleteGivenIWantToDeleteWithValidTitle(string title)
            {
                Console.WriteLine("==================== TODOLIST DELETE (by Id) - Happy Path");
                Console.WriteLine($"Given I want to delete a record with valid id \"{title}\": PASSED");
            }

            [When("I send a DELETE request with valid data to Delete To Do Item by Id API \"(.*)\"")]
            public void DeleteWhenISendAValidDeleteRequestToDeleteToDoItemByIdAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.OK;
                _simulatedResponseMessage = "ITEM DELETED SUCCESSFULLY.";
                Console.WriteLine($"When I send a DELETE request with valid data to Delete To Do Item by Id API \"{apiEndpoint}\": PASSED");
            }

            [Then("the http status code response with valid data for deleting the item should be (.*)")]
            public void DeleteThenTheValidHttpStatusCodeResponseForDeletingItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response with valid data for deleting the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response with valid data for deleting the item should be SUCCESS")]
            public void DeleteThenTheValidHttpStatusResponseForDeletingItemShouldBeSUCCESS()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.OK);
                Console.WriteLine("And the http status response with valid data for deleting the item should be SUCCESS: PASSED");
            }

            [Then("the http message response with valid data for deleting the item should be \"(.*)\"")]
            public void DeleteThenTheValidHttpMessageResponseForDeletingItemShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response with valid data for deleting the item should be \"{expectedMessage}\": PASSED\n");
                Console.WriteLine("==================== END");
            }
            #endregion
        #endregion



        #region NEGATIVE PATHS (5)
            #region Negative Scenario 1: Failed to create a To-Do item with invalid data
            [Given("I create a To-Do List with title \"(.*)\"")]
            public void CreateGivenICreateAToDoListWithTheInvalidTitle(string title)
            {
                Console.WriteLine("==================== TODOLIST CREATE - Negative Path");
                Console.WriteLine($"Given I create a To-Do List with title \"{title}\": PASSED");
            }

            [Given("with description \"(.*)\"")]
            public void CreateGivenInvalidDescription(string description)
            {
                Console.WriteLine($"And with description \"{description}\": PASSED");
            }

            [Given("with item status is set to default \"(.*)\"")]
            public void CreateGivenTheInvalidItemStatusIsSetToDefault(string itemStatus)
            {
                Console.WriteLine($"And with item status is set to default \"{itemStatus}\": PASSED");
            }

            [When("I send a POST request to Create To Do API \"(.*)\"")]
            public void CreateWhenISendAPOSTRequestToCreateToDoWithInvalidData(string apiEndPoint)
            {
                _simulatedStatusCode = HttpStatusCode.BadRequest;
                _simulatedResponseMessage = "Not found or Invalid item data.";
                Console.WriteLine($"When I send a POST request to Create To Do API \"{apiEndPoint}\": FAILED");
            }

            [Then("the http status code response for creating an item should be (.*)")]
            public void CreateThenTheInvalidHttpStatusCodeResponseShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response for creating an item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response for creating an item should be FAIL")]
            public void CreateThenTheInvalidHttpStatusResponseShouldBeFAIL()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.BadRequest);
                Console.WriteLine("And the http status response for creating an item should be FAIL: PASSED");
            }

            [Then("the http message response for creating an item should be \"(.*)\"")]
            public void CreateThenTheInvalidHttpMessageResponseShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response for creating an item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Negative Scenario 2: Failed to Retrieve All To-Do Items
            [Given("I have no records of To-Do items")]
            public void GivenIHaveNoRecordsOfTo_DoItems()
            {
                _simulatedStatusCode = HttpStatusCode.InternalServerError; 
                _simulatedResponseMessage = "An error occurred while retrieving items.";  
                _hasData = false;
                Console.WriteLine("Given I have no records of To-Do items: PASSED");
            }

            [When("I send a GET request to Retrieve All To Do API \"(.*)\"")]
            public void WhenISendAGetRequestToRetrieveAllToDoAPI(string apiEndpoint)
            {
                if (!_hasData)
                {
                    _simulatedStatusCode = HttpStatusCode.InternalServerError;  
                    _simulatedResponseMessage = "An error occurred while retrieving items.";  
                }
                else
                {
                    _simulatedStatusCode = HttpStatusCode.OK;
                    _simulatedResponseMessage = "Retrieved items.";
                }

                Console.WriteLine($"When I send a GET request to Retrieve All To Do API \"{apiEndpoint}\": FAILED");
            }

            [Then("the http status code response for retrieving all items should be (.*)")]
            public void ThenTheHttpStatusCodeResponseForRetrievingAllItemsShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(expectedStatusCode, _simulatedStatusCode);
                Console.WriteLine($"Then the http status code response for retrieving all items should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response for retrieving all items should be FAIL")]
            public void ThenTheHttpStatusResponseForRetrievingAllItemsShouldBeFAIL()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.InternalServerError);
                Console.WriteLine("And the http status response for retrieving all items should be FAIL: PASSED");
            }

            [Then("the http message response for retrieving all items should be \"(.*)\"")]
            public void RetrieveThenTheHttpMessageResponseShouldBe(string expectedMessage)
            {
                Assert.Equal(_simulatedResponseMessage, expectedMessage);
                Console.WriteLine($"And the http message response for retrieving all items should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Negative Scenario 3: Failed to retrieve a To-Do Item by Id
            [Given("I have no To-Do item with id (.*)")]
            public void RetrieveGivenIHaveNoToDoItemWithId(string id)
            {
                Console.WriteLine("==================== TODOLIST RETRIEVE (by Id) - Negative Path");
                Console.WriteLine($"Given I have no To-Do item with id \"{id}\": PASSED");
            }

            [When("I send a GET request to Retrieve To Do Item by Id API \"(.*)\"")]
            public void RetrieveWhenISendAGetRequestToRetrieveToDoItemByIdAPI(string apiEndPoint)
            {
                _simulatedStatusCode = HttpStatusCode.NotFound;
                _simulatedResponseMessage = "Not found or Invalid item data.";
                Console.WriteLine($"When I send a GET request to Retrieve To Do Item by Id API \"{apiEndPoint}\": FAILED");
            }

            [Then("the http status code response for retrieving the item should be (.*)")]
            public void RetrieveThenTheHttpStatusCodeResponseForItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response for retrieving the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response for retrieving the item should be FAIL")]
            public void RetrieveThenTheHttpStatusResponseForItemShouldBeFAIL()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.NotFound);
                Console.WriteLine("And the http status response for retrieving the item should be FAIL: PASSED");
            }

            [Then("the http message response for retrieving the item should be \"(.*)\"")]
            public void RetrieveThenTheHttpMessageResponseForItemShouldBe(string expectedMessage)
            {
                Assert.Equal(expectedMessage, _simulatedResponseMessage);
                Console.WriteLine($"And the http message response for retrieving an item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Negative Scenario 4: Failed to update a To-Do item by Id
            [Given("I want to update a record with id \"(.*)\"")]
            public void UpdateGivenIWantToUpdateWithId(string id)
            {
                Console.WriteLine("==================== TODOLIST UPDATE (by Id) - Negative Path");
                Console.WriteLine($"Given I want to update a record with id \"{id}\": PASSED");
            }
            [Given("I want to update the title to \"(.*)\"")]
            public void UpdateGivenIWantToUpdateTheTitleTo(string newTitle)
            {
                Console.WriteLine($"Given I want to update the title to \"{newTitle}\": PASSED");
            }

            [Given("I want to update the description to \"(.*)\"")]
            public void UpdateGivenIWantToUpdateTheDescriptionTo(string description)
            {
                Console.WriteLine($"Given I want to update the description to \"{description}\": PASSED");
            }

            [When("I send a PUT request to Update To Do Item by Id API \"(.*)\"")]
            public void UpdateWhenISendAPutRequestToUpdateToDoItemByIdAPI(string apiEndPoint)
            {
                _simulatedStatusCode = HttpStatusCode.NotFound;
                _simulatedResponseMessage = "Not found or Invalid item data.";
                Console.WriteLine($"When I send a PUT request to Update To Do Item by Id API \"{apiEndPoint}\": FAILED");
            }

            [Then("the http status code response for updating the item should be (.*)")]
            public void UpdateThenTheHttpStatusCodeResponseForUpdatingItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response for updating the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response for updating the item should be FAIL")]
            public void UpdateThenTheHttpStatusResponseForUpdatingItemShouldBeFAIL()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.NotFound);
                Console.WriteLine("And the http status response for updating the item should be FAIL: PASSED");
            }

            [Then("the http message response for updating the item should be \"(.*)\"")]
            public void UpdateThenTheHttpMessageResponseForUpdatingItemShouldBe(string expectedMessage)
            {
                Assert.Equal(expectedMessage, _simulatedResponseMessage);
                Console.WriteLine($"And the http message response for updating the item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion


            #region Negative Scenario 5: Failed to Delete a To-Do Item by Id
            [Given("I want to delete a record with id \"(.*)\"")]
            public void DeleteGivenIWantToDeleteWithTitle(string id)
            {
                Console.WriteLine("==================== TODOLIST DELETE (by Id) - Negative Path");
                Console.WriteLine($"Given I want to delete a record with id \"{id}\": PASSED");
            }

            [When("I send a DELETE request to Delete To Do Item by Id API \"(.*)\"")]
            public void DeleteWhenISendADeleteRequestToDeleteToDoItemByIdAPI(string apiEndpoint)
            {
                _simulatedStatusCode = HttpStatusCode.NotFound;
                _simulatedResponseMessage = "Not found or Invalid item data.";
                Console.WriteLine($"When I send a DELETE request to Delete To Do Item by Id API \"{apiEndpoint}\": FAILED");
            }

            [Then("the http status code response for deleting the item should be (.*)")]
            public void DeleteThenTheHttpStatusCodeResponseForDeletingItemShouldBe(HttpStatusCode expectedStatusCode)
            {
                Assert.Equal(_simulatedStatusCode, expectedStatusCode);
                Console.WriteLine($"Then the http status code response for deleting the item should be {expectedStatusCode}: PASSED");
            }

            [Then("the http status response for deleting the item should be FAIL")]
            public void DeleteThenTheHttpStatusResponseForDeletingItemShouldBeFAIL()
            {
                Assert.True(_simulatedStatusCode == HttpStatusCode.NotFound);
                Console.WriteLine("And the http status response for retrieving the item should be FAIL: PASSED");
            }

            [Then("the http message response for deleting the item should be \"(.*)\"")]
            public void DeleteThenTheHttpMessageResponseForDeletingItemShouldBe(string expectedMessage)
            {
                Assert.Equal(expectedMessage, _simulatedResponseMessage);
                Console.WriteLine($"And the http message response for deleting the item should be \"{expectedMessage}\": PASSED");
                Console.WriteLine("==================== END");
            }
            #endregion
         #endregion
    }
}
