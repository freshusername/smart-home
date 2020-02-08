$(document).ready(function () {
    $(".grid-stack").gridstack();

    let elements = $(".grid-stack-item");
    for (var i = 0; i < elements.length; i++) {
        visualLock(elements[i].id);
    }
});

let arrToObj = arr => {
    let obj = {}
    for (const i in arr) {
        obj[i] = arr[i]
    }
    return obj
}

$('.grid-stack').on('change', function (event, items) {
    let options = [];
    options = items.map(i => {
        return { id: parseInt(i.el[0].id), x: i.x, y: i.y, width: i.width, height: i.height }
    });

    $.ajax({
        type: "POST",
        url: $(".grid-stack").data("save-url"),
        data: { options: arrToObj(options) }
    });
});

let remove = (id) => {
    if (confirm("Are you sure you wish to delete this widget?") == true) {
        let grid = $(".grid-stack").data('gridstack');
        let item = $("#" + id)[0];
        grid.removeWidget(item);

        $.ajax({
            type: "POST",
            url: "/ReportElement/DeleteReportElement",
            data: { id: id }
        });
    }
}

var lock = (id) => {
    grid = $(".grid-stack").data("gridstack");

    let item = $("#" + id);
    if (item != null) {
        if (item[0].dataset.locked == "False") {
            item[0].dataset.locked = "True"
        }
        else {
            item[0].dataset.locked = "False"
        }

        let flag = item[0].dataset.locked == "True";
        grid.resizable(item, !flag);
        grid.movable(item, !flag);
        grid.locked(item, flag);

        $(".i-button", item).toggleClass("fa-lock text-danger");
        $(".i-button", item).toggleClass("fa-unlock text-success");

        $.ajax({
            type: "POST",
            url: "/ReportElement/LockReportElement",
            data: { id: id }
        });
    }
}

var visualLock = (id) => {
    grid = $(".grid-stack").data("gridstack");
    let item = $("#" + id);
    if (item != null) {
        //TODO: Create separate func
        let flag = item[0].dataset.locked == "True";
        grid.resizable(item, !flag);
        grid.movable(item, !flag);
        grid.locked(item, flag);
        if (flag) {
            $(".i-button", item).addClass("fa-lock text-danger");
            $(".i-button", item).removeClass("fa-unlock text-success");
        }
        else {
            $(".i-button", item).removeClass("fa-lock text-danger");
            $(".i-button", item).addClass("fa-unlock text-success");
        }
    }
}

