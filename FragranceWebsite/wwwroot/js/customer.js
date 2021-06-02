var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/customers/GetAllCustomers",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "firstName", "width": "14%" },
            { "data": "lastName", "width": "14%" },
            { "data": "phone", "width": "14%" },
            { "data": "addressType", "width": "14%" },
            { "data": "address", "width": "14%" },
            {
                "data": "customerId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/customers/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/customers/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='fa fa-trash'></i></a>
                                </div>
                            `;
                }, "width": "30%"
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
