var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/fragrances/GetAllFragrances",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "brand", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "releaseYear", "width": "10%" },
            { "data": "perfumer", "width": "10%" },
            { "data": "type", "width": "10%" },
            { "data": "mainNotes", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "fragranceId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/fragrances/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/fragrances/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='fa fa-trash'></i></a>
                                </div>
                            `;
                }, "width": "10%"
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