$(function () {
    $(".btn-success").click(function (event) {
        var id = Number(event.currentTarget.id);

        $.ajax({
            type: "POST",
            url: "/Admin/ConfirmReservation",
            data: { id:id },
            success: function (status) {
                if (status.status == true) {
                    window.location.reload();
                    alert("done");
                } else {
                    alert("fail to confirm");
                }
            },
            error: function () {
                alert("error");
            }
            
        });
    });

    $(".btn-danger").click(function (event) {
        var id = Number(event.currentTarget.id);

        $.ajax({
            type: "POST",
            url: "/Admin/DeleteReservation",
            data: { id: id },
            success: function (status) {
                if (status.status == true) {
                    window.location.reload();
                    alert("done");
                } else {
                    alert("fail to confirm");
                }
            },
            error: function () {
                alert("error");
            }

        });
    });

});