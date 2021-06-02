var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/sales/GetAllSales",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": null, render: function (data, type, row) {
                    return row.customer.firstName + ' ' + row.customer.lastName;
                }, "width": "12%"
            },
            { "data": "customer.address", "width": "12%" },
            {
                "data": null, render: function (data, type, row) {
                    return row.fragrance.brand + ' ' + row.fragrance.name;
                }, "width": "10%"
            },
            { "data": "fragrance.price", "width": "12%" },
            { "data": "quantity", "width": "12%" },
            { "data": "dateTime", "width": "12%" },
            {
                "data": "fragrance.price", render: function (data, type, row) {
                    if (row.quantity >= 2)
                        return 0.9 * (data * row.quantity) + " BGN";
                    return data * row.quantity + " BGN";
                }, "width": "12%"
            },
            {
                "data": "saleId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/sales/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/sales/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='fa fa-trash'></i></a>
                                </div>
                            `;
                }, "width": "28%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
            });
        }
    });
}