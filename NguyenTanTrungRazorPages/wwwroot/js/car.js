
var connection = new signalR.HubConnectionBuilder().withUrl("/carHub").build();

connection.start().then(function () {
    console.log("SignalR connected.");
    connection.invoke("GetAllCars");
}).catch(function (err) {
    console.error(err.toString());
});

connection.on("ReceiveAllCars", function (cars) {
    console.log("Received cars:", cars);

    var tableBody = $("#carList tbody");
    tableBody.empty();

    for (var i = 0; i < cars.length; i++) {
        var car = cars[i];

        var row = $("<tr></tr>");
        row.append("<td>" + car.carId + "</td>");
        row.append("<td>" + car.carName + "</td>");
        row.append("<td>" + car.carModelYear + "</td>");
        row.append("<td>" + car.color + "</td>");
        row.append("<td>" + car.capacity + "</td>");
        row.append("<td>" + car.description + "</td>");
        row.append("<td>" + car.importDate + "</td>");
        row.append("<td>" + car.rentPrice + "</td>");
        row.append("<td>" + car.producerId + "</td>");
        row.append("<td><button class='delete-car-btn' data-car-id='" + car.carId + "'>Delete</button></td>");

        tableBody.append(row);
    }
});





$("#addCarBtn").click(function () {
    $("#addCarPopup").show();
});

$("#addCarSubmit").click(function () {
    var carName = $("#carName").val();
    var carModelYear = $("#carModelYear").val();
    var color = $("#color").val();
    var capacity = $("#capacity").val();
    var description = $("#description").val();
    var importDate = $("#importDate").val();
    var rentPrice = $("#rentPrice").val();
    var producerId = $("#producerId").val();

    console.log(producerId);
    
    // Invoke the AddCar function in the server-side CarHub
    connection.invoke("CreateNewCar", carName, carModelYear, color, capacity, description, importDate, rentPrice, producerId)
        .then(function () {
            // Success
            $("#addCarPopup").hide();
            // Clear input values
            $("#carName").val("");
            $("#carModelYear").val("");
            $("#color").val("");
            $("#capacity").val("");
            $("#description").val("");
            $("#importDate").val("");
            $("#rentPrice").val("");
            
        })
        .catch(function (err) {
            // Error
            $("#error-message").text("Save error");
        });
});

connection.on("ReceiveNewCar", function (car) {
    console.log("Received new car:", car);

    var tableBody = $("#carList tbody");

    var row = $("<tr></tr>");
    row.append("<td>" + car.carId + "</td>");
    row.append("<td>" + car.carName + "</td>");
    row.append("<td>" + car.carModelYear + "</td>");
    row.append("<td>" + car.color + "</td>");
    row.append("<td>" + car.capacity + "</td>");
    row.append("<td>" + car.description + "</td>");
    row.append("<td>" + car.importDate + "</td>");
    row.append("<td>" + car.rentPrice + "</td>");
    row.append("<td>" + car.producerId + "</td>");
    row.append("<td><button class='delete-car-btn' data-car-id='" + car.carId + "'>Delete</button></td>");

    tableBody.append(row);
});


$(document).on("click", ".delete-car-btn", function () {
    var carId = $(this).data("car-id");
    connection.invoke("DeleteCar", carId)
        .then(function () {
            console.log("Car deleted successfully.");
        })
        .catch(function (err) {
            console.error("Error deleting car:", err.toString());
        });
});

connection.on("ReceiveDeletedCar", function (carId) {
    console.log("Car deleted:", carId);

    $("#carList tbody tr").each(function () {
        var rowCarId = $(this).find("td:first").text();
        if (rowCarId == carId) {
            $(this).remove();
        }
    });
});









