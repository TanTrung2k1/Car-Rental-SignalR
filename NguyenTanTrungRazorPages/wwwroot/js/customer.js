var connection = new signalR.HubConnectionBuilder().withUrl("/customerHub").build();

connection.start().then(function () {
    console.log("SignalR connected.");
    connection.invoke("GetAllCustomer");
}).catch(function (err) {
    console.error(err.toString());
});

connection.on("ReceiveAllCustomers", function (customers) {
    console.log("Received customers:", customers);

    var tableBody = $("#customerList tbody");
    tableBody.empty();

    for (var i = 0; i < customers.length; i++) {
        var cus = customers[i];

        var row = $("<tr></tr>");
        row.append("<td>" + cus.customerName + "</td>");
        row.append("<td>" + cus.mobile + "</td>");
        row.append("<td>" + cus.customerEmail + "</td>");
        row.append("<td>" + cus.identityCard + "</td>");
        row.append("<td>" + cus.licenceNumber + "</td>");
        row.append("<td>" + cus.licenceDate + "</td>");
        

        tableBody.append(row);
    }
});

$("#addBtn").click(function () {
    $("#addCustomerPopup").show();
});

$("#addCustomerSubmit").click(function () {
    var customerName = $("#customerName").val();
    var mobile = $("#mobile").val();
    var customerEmail = $("#customerEmail").val();
    var customerPassword = $("#customerPassword").val();
    var identityCard = $("#identityCard").val();
    var licenceNumber = $("#licenceNumber").val();
    var licenceDate = $("#licenceDate").val();

    
    
    // Invoke the AddCustomer function in the server-side CustomerHub
    //connection.invoke("CreateNewCustomer", customerName, mobile, customerEmail, customerPassword, identityCard, licenceNumber, licenceDate);
    connection.invoke("CreateNewCustomer01", customerName, mobile, customerEmail, customerPassword, identityCard, licenceNumber, licenceDate)
        .then(function () {
            // Success
            $("#addCustomerPopup").hide();
            // Clear input values
            $("#customerName").val("");
            $("#mobile").val("");
            $("#customerEmail").val("");
            $("#customerPassword").val("");
            $("#identityCard").val("");
            $("#licenceNumber").val("");
            $("#licenceDate").val("");
        })
        .catch(function (err) {
            // Error
            $("#error-message").text("Save error");
        });
});

connection.on("ReceiveNewCustomer", function (cus) {
    console.log("Received new customer:", cus);

    var tableBody = $("#customerList tbody");

    var row = $("<tr></tr>");
    row.append("<td>" + cus.customerName + "</td>");
    row.append("<td>" + cus.mobile + "</td>");
    row.append("<td>" + cus.customerEmail + "</td>");
    row.append("<td>" + cus.identityCard + "</td>");
    row.append("<td>" + cus.licenceNumber + "</td>");
    row.append("<td>" + cus.licenceDate + "</td>");

    tableBody.append(row);
});