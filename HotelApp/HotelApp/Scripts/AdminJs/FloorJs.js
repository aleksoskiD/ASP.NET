$(function () {

    // Create Floor
    $(".createFloor").click(function (event) {
        //alert("Hellu");
        //event.preventDefault();
        $("#floorForm").dialog("open");
    });

    $("#floorForm").dialog({
        autoOpen: false,
        width: 600,
        height: 400,
        modal: true,
        close: function (event, ui) {
            //dialog.dialog("close");
            $(this).dialog("close");
        },
        buttons:
        {
            "Add": function () {
                var roomsQuantity = $("#roomsQuantity").val();
                var isActive = $("#IsActiveFloor").is(':checked');
                var floor = {
                    NumberOfRooms: roomsQuantity,
                    IsActive: isActive
                }
                $.ajax(
                    {
                        type: "POST", //HTTP POST Method  
                        url: "/Admin/CreateFloor", // Controller/View   
                        data: floor,
                        success: function (status) {
                            // alert("done");
                            var tt = status;
                            var id = status.ID;
                            var floorNo = status.FloorNo;
                            var numOfRooms = status.NumberOfRooms;
                            var isActive = status.IsActive;

                            if (status != false) {
                               // AppendFloor(status);
                                window.location.reload();
                                alert("done");
                            }

                            //window.location.reload();
                            alert("fail");
                            $("#floorForm").dialog("close");
                        },
                        error: function () {
                             alert("fail");
                            //window.location.reload();
                            $("#floorForm").dialog("close");
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

    // Deactivate Floor
   $(".deactivateFloor").click(function (event) {
       var floorId = event.currentTarget.id;
       $.ajax({
            type: "POST",
            url: "/Admin/DeactivateFloor",
            data: {FloorId:floorId},
            success: function (status) {
                if (status == null) {
                    alert("fail");
                }
                window.location.reload();
            },
            error: function () {
                 alert("fail");
            }
        });
        //alert("Deactivate Floor. ID = " + id);
    });


    // Activate Floor
    $(".activateFloor").click(function (event) {
        var floorId = event.currentTarget.id;
        $.ajax({
            type: "POST",
            url: "/Admin/ActivateFloor",
            data: { FloorId: floorId },
            success: function (status) {
                if (status == false)
                {
                    alert("fail");
                }
                window.location.reload();
            },
            error: function () {
                alert("fail");
            }
        });
        //alert("Deactivate Floor. ID = " + id);
    });

    // Edit Floor
    $(".editFloor").click(function (event) {
        event.preventDefault();
        var id = event.currentTarget.id;

        $.ajax({
            type: "GET", //HTTP POST Method  
            url: "/Admin/GetFloor?", // Controller/View   
            data: { "id": id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (values) {
                $("#floorNoEdit").val(values.floorNo);
                $("#roomsQuantityEdit").val(values.numOfRooms);
                if (values.isActive == true) {
                    $("#IsActiveFloorEdit").prop('checked', true);
                } else {
                    $("#IsActiveFloorEdit").removeProp('checked', true);
                }
                
                $("#floorIdEdit").val(values.floorId);
                //alert("done");
            },
            error: function () {
                alert("fail");
            }
        });

        

        $("#floorFormEdit").dialog("open");
       // alert("Edit Floor id " + id);
    });

    $("#floorFormEdit").dialog({
        autoOpen: false,
        width: 600,
        height: 400,
        modal: true,
        close: function (event, ui) {
            //dialog.dialog("close");
            $(this).dialog("close");
        },
        buttons:
        {
            "Save": function () {
                var floorNum = $("#floorNoEdit").val();
                var roomsQuantity = $("#roomsQuantityEdit").val();
                var isActive = $("#IsActiveFloorEdit").is(':checked');
                var floorId = $("#floorIdEdit").val();
                var floor = {
                    ID: floorId,
                    FloorNo: floorNum,
                    NumberOfRooms: roomsQuantity,
                    IsActive: isActive
                }
                $.ajax(
                    {
                        type: "POST", //HTTP POST Method  
                        url: "/Admin/UpdateFloor", // Controller/View   
                        data: floor,
                        success: function (status) {
                            if (status == false) {
                                alert("Error");
                                $("#floorFormEdit").dialog("close");
                            } else {
                                // alert("done");
                                window.location.reload();
                                $("#floorFormEdit").dialog("close");
                            }
                        },
                        error: function () {
                            // alert("fail");
                            //window.location.reload();
                            //$("#floorFormEdit").dialog("close");
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