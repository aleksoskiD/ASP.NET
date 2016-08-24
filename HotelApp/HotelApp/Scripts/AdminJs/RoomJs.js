$(function () {

    // Create Room
    $(".createRoom").click(function (event) {
        //alert("Hellu");
        //event.preventDefault();
        $("#roomForm").dialog("open");
    });

    $("#roomForm").dialog({
        autoOpen: false,
        width: 650,
        height: 450,
        modal: true,
        close: function (event, ui) {
            //dialog.dialog("close");
            $(this).dialog("close");
        },
        buttons:
        {
            "Add": function () {
                var roomType = $("#roomType").val();
                var isActive = $("#IsActiveRoom").is(':checked');
                var floorId = $("#FloorId").val();
                var description = $("#description").val();
                var quantity = $("#numOfRooms").val();

                $.ajax({
                    type: "POST",               //HTTP POST Method  
                    url: "/Admin/CreateRoom",   // Controller/View
                    data: {
                        Type: roomType,
                        IsActive: isActive,
                        Quantity: quantity,
                        FloorId: floorId,
                        Description: description
                    },
                    success: function (status) {
                        // alert("done");
                        window.location.reload();
                        $("#roomForm").dialog("close");
                    },
                    error: function () {
                        // alert("fail");
                        $("#roomForm").dialog("close");
                    }
                });
            },
            "Cancel": function () {
                //cancel
                //$("#floortForm").dialog("close");
                $(this).dialog("close");
            }
        }
    });


    // Deactivate Room
    $(".deactivateRoom").click(function (event) {
        var id = event.currentTarget.id;
        $.ajax({
            type: "POST",
            url: "/Admin/DeactivateRoom",
            data: { RoomId: id },
            success: function (status) {
                alert("done");
                window.location.reload();
            },
            error: function () {
                alert("fail");
                window.location.reload();
            }
        });
        //alert("Edit room " + id);
    });


    // Activate Room
    $(".activateRoom").click(function (event) {
        var id = event.currentTarget.id;
        $.ajax({
            type: "POST",
            url: "/Admin/ActivateRoom",
            data: { RoomId: id },
            success: function (status) {
                alert("done");
                window.location.reload();
            },
            error: function () {
                alert("fail");
                window.location.reload();
            }
        });
        //alert("Edit room " + id);
    });


    // Edit Room
    $(".editRoom").click(function (event) {
        event.preventDefault();
        var id = event.currentTarget.id;
        
        $.ajax({
            type: "GET", //HTTP POST Method  
            url: "/Admin/GetRoom?", // Controller/View   
            data: { "id": id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (values) {
                $("#roomIdEdit").val(values.roomId);
                $("#roomTypeEdit").val(values.roomType);
                if (values.isActive == true) {
                    $("#IsActiveRoomEdit").prop('checked', true);
                } else {
                    $("#IsActiveRoomEdit").removeProp('checked', true);
                }
                if (values.isReserved == true) {
                    $("#IsReservedRoomEdit").prop('checked', true);
                } else {
                    $("#IsReservedRoomEdit").removeProp('checked', true);
                }
                $(".editFloorId").val(values.floorId);
                $("#roomDescriptionEdit").val(values.description);
                //alert("done");
            },
            error: function () {
                alert("fail");
            }
        });

        $("#roomFormEdit").dialog("open");
        //alert("Edit room " + id);
    });

    $("#roomFormEdit").dialog({
        autoOpen: false,
        width: 600,
        height: 500,
        modal: true,
        close: function (event, ui) {
            //dialog.dialog("close");
            $(this).dialog("close");
        },
        buttons:
        {
            "Save": function () {
                var par = $(this).parent().parent();
                var floorId = $(".editFloorId").val();
                var roomId =   $("#roomIdEdit").val();
                var roomType = $("#roomTypeEdit").val();
                var isActive = $("#IsActiveRoomEdit").is(':checked');
                var isReserved = $("#IsReservedRoomEdit").is(':checked');
                var description = $("#roomDescriptionEdit").val();
                var room = {
                    ID: roomId,
                    FloorId: floorId,
                    RoomType: roomType,
                    IsActive: isActive,
                    IsReserved: isReserved,
                    Description: description
                }
                $.ajax(
                    {
                        type: "POST", //HTTP POST Method  
                        url: "/Admin/UpdateRoom", // Controller/View   
                        data: room,
                        success: function (status) {
                            if (status == false) {
                                alert("Error");
                                $("#floorFormEdit").dialog("close");
                            } else {
                                // alert("done");
                                window.location.reload();
                                //$("#floorFormEdit").dialog("close");
                            }
                        },
                        error: function () {
                            alert("fail");
                           // window.location.reload();
                            $("#floorFormEdit").dialog("close");
                        }
                    });
            },
            "Cancel": function () {
                //cancel
                //$("#floortForm").dialog("close");
                $(this).dialog("close");
            }
        }
    });
});