$(function () {
    $("#createReservation").click(function () {
        var guestId = $("#GuestId").val();
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();
        var roomType = $('input[name="RoomId"]:checked').val();

        var reservation = {
            GuestId: guestId,
            StartDate: startDate,
            EndDate: endDate,
            RoomType: roomType
        };

        $.ajax({
            type: "POST",
            url: "/Reservation/CreateReservation",
            data: reservation,
            success: function (success) {
                if (success.success != true) {
                    alert("fail");
                } else {
                    alert("done");
                    window.location.replace("/Reservation/Index/" + guestId);
                }
            },
            error: function (status) {
                alert("fail");
            }
        });

    });

    $(".cancelReservation").click(function () {
        var reservationId = Number(event.currentTarget.id);

        $.ajax({
            type: "POST",
            url: "/Reservation/CancelReservation",
            data: { id: reservationId },
            success: function (success) {
                if (success.success != true) {
                    alert("fail");
                } else {
                    window.location.reload();
                    alert("done");
                }
            },
            error: function (status) {
                alert("fail");
            }
        });
    });

    $("#searchDate").click(function () {
        var startDate = $("#startDateSearch").val();
        var endDate = $("#endDateSearch").val();

    });
});