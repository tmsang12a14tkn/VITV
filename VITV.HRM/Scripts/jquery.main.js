var employeesSource = null;
var equipmentSource = null;

var getEmployeeSource = function (callback) {
    if (employeesSource == null) {
        $.get("/Employee/GetList", function (data) {
            employeesSource = data;
            if (callback != null) callback();
        })
    }
    else if (callback != null)
        callback();
}
var getEquipmentSource = function (callback) {
    if (equipmentSource == null) {
        $.get("/Equipment/GetList", function (data) {
            equipmentSource = data;
            if (callback != null) callback();
        });
    }
    else if (callback != null)
        callback();
}

var getSources = function(callback)
{
    getEmployeeSource(function()
    {
        getEquipmentSource(function ()
        {
            if (callback != null)
                callback();
        })
    })
}

var registerEditable = function ($elements) {
    if (employeesSource != null && equipmentSource != null) {
        $elements.each(function (i, el) {
            if ($(el).data('type') == 'select2') {
                var source = $(el).data('sourcetype') == 'employees' ? employeesSource : equipmentSource;
                $(el).editable({
                    inputclass: 'input-large',
                    source: source,
                    select2: 
                    {
                        multiple: true
                    }
                });
            }
            else if ($(el).data('type') == "wysiwyg") {
                $(el).editable(
                {
                    wysiwyg: {
                    }
                });
            }
            else if ($(el).data('type') == "combodate") {
                $(el).editable({
                });
            }
            else
                $(el).editable();
        })
    }
    else
        getSources(function () {
            registerEditable($elements);
        })

}

var registerBtnAddJob = function ($elements) {
    //$(".add-item-btn")
    $elements.click(function () {
        var listId = $(this).data('listid');
        $('.new-item-editable[data-listid=' + listId + ']').editable('submit', {
            url: '/Job/Add',
            ajaxOptions: {
                dataType: 'json' //assuming json response
            },
            success: function (data, config) {
                if (data && data.checkItemId) {  //record created, response like {"id": 2}
                    //set pk
                    //$(this).editable('option', 'pk', data.id);
                    $('#checklist-' + data.checklistId).load("/Checklist/Details/" + data.checklistId, function () {
                        registerEditable($('#checklist-' + data.checklistId + ' .item-editable'));
                        registerEditable($('#checklist-' + data.checklistId + ' .new-item-editable'));
                        registerBtnAddJob($('#checklist-' + data.checklistId + ' .add-item-btn'));
                    });
                }
            }
        });
    });
}

var registerJobList = function ()
{
    registerBtnAddJob($(".add-item-btn"));

    $('.dropdown-menu').find('.create-checklist-form').click(function (e) {
        e.stopPropagation();
    });

    getSources(function () {
        registerEditable($('.item-editable'));
        registerEditable($('.new-item-editable'));
        //registerEditable($('.task-editable'));
        registerEditable($('.joblist-editable'));
    });

    $('[data-rel=popover]').popover({ html: true });

    $('.create-checklist-form').ajaxForm({
        success: function (responseText, statusText, xhr, $form) {
            if (responseText.success == true)
                location.reload();
        }
    });

    $('.delete-checklist-form').ajaxForm({
        success: function (responseText, statusText, xhr, $form) {
            if (responseText.success == true)
                location.reload();
        }
    });


    $(document).on('change', '.item-checkbox', function () {
        var item_id = $(this).data('id');

        $.post("/Job/Check?id=" + item_id + "&value=" + $(this).is(":checked"), function (data) {
            console.log(data);
            $("#checklist-count-" + data.checklistId).html(data.done + '/' + data.all);
        });
    });
}

$.extend($.gritter.options, {
    position: 'bottom-right',
    time: 12000
});


jQuery(function ($) {
    $('.modal.aside').ace_aside();

    $('#aside-inside-modal').addClass('aside').ace_aside({ container: '#my-modal > .modal-dialog' });

    $(document).one('ajaxloadstart.page', function (e) {
        //in ajax mode, remove before leaving page
        //alert("asd");
        $('.modal.aside').remove();
        $(window).off('.aside')
    });

    window.hubReady = $.connection.hub.start();
    $('#job-view-modal').on("show.bs.modal", function (e) {
        var $modalContent = $(this).find(".modal-content");
        var registElements = function () {
            $("#response-items").scrollTop($("#response-items")[0].scrollHeight);
            $('#input-response-attachments').ace_file_input({
                no_file: 'No File ...',
                btn_choose: 'Choose',
                btn_change: 'Change',
                droppable: false,
                onchange: null,
                thumbnail: false //| true | large
                //whitelist:'gif|png|jpg|jpeg'
                //blacklist:'exe|php'
                //onchange:''
                //
            });
            $('#input-item-attachments').ace_file_input({
                style: 'well',
                btn_choose: 'Drop files here or click to choose',
                btn_change: null,
                no_icon: 'ace-icon fa fa-cloud-upload',
                droppable: true,
                thumbnail: 'small',
                preview_error: function (filename, error_code) {
                }

            }).on('change', function () {
            });

            $('.uploaded-attachment .remove').on('click', function () {
                $(this).parent().remove();
            });

            $('#response-form').ajaxForm(function (data) {
                console.log(data);
                $('#response-form')[0].reset();
                $.get("/JobResponse/Details/" + data.id, function (result) {
                    $("#response-items").append(result);
                    $("#response-items").scrollTop($("#response-items")[0].scrollHeight);
                })
            });
            $('.edit-item-attachments-form').ajaxForm(function (data) {
                $modalContent.load('/job/details/' + data.id, registElements);
            });

            //$('.edit-job-employees-btn').on('click', function (e) {
            //    e.stopPropagation();
            //    var id = $(this).data('id');
            //    var btn = this;
            //    $(this).hide();
            //    $('#job-employees-' + id + ' .profile-users').addClass('hidden');
            //    $('#job-employees-' + id + ' .employees-edit').removeClass('hidden');
            //    $('#job-employees-' + id + ' .item-editable').editable('show');
            //    $('#job-employees-' + id + ' .item-editable').editable({
            //        success: function (data, config) {
            //            console.log(data, config);
            //            $(btn).show();
            //            $('#job-employees-' + id + ' .profile-users').removeClass('hidden');
            //            $('#job-employees-' + id + ' .employees-edit').addClass('hidden');
            //            $('#job-employees-' + id + ' .item-editable').editable('hide');
            //        }
            //    }
            //    );
            //})
            //$('#change-request-status-form').ajaxForm(function (data) {
            //    console.log(data);
            //    $modalContent.load('/job/details/' + data.id, registElements);
            //})
            getSources(function () {
                registerEditable($modalContent.find('.item-editable'));
                registerEditable($modalContent.find('.new-item-editable'));
                registerEditable($modalContent.find('.joblist-editable'));
            });
        };
        $modalContent.load($(e.relatedTarget).data('href'), registElements);
    });

    $('#request-modal').on("show.bs.modal", function (e) {
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $("#tbEmployees").chosen({
                width: "100%",
                no_results_text: "Không có kết quả nào!"
            });

            $('#tbEmployees').on('change', function (evt, params) {
                var ids = "";
                var selectedIds = $("#tbEmployees").chosen().val();

                if (selectedIds != null) {
                    $.each(selectedIds, function (index, value) {
                        ids = ids + ", " + value;
                    });
                }
                $("#ReceivedEmployeeIds").val(ids);
            });

            //$('#attachments').ace_file_input({
            //    style: 'well',
            //    btn_choose: 'Drop files here or click to choose',
            //    btn_change: null,
            //    no_icon: 'ace-icon fa fa-cloud-upload',
            //    droppable: true,
            //    thumbnail: 'small',
            //    preview_error: function (filename, error_code) {
            //    }

            //}).on('change', function () {
            //});

            //$('.uploaded-attachment .remove').on('click', function () {
            //    $(this).parent().remove();
            //});
            $('#editor-content').ace_wysiwyg().prev().addClass('wysiwyg-style2');
            $(".request-form").ajaxForm(
            {
                beforeSerialize: function()
                {
                    $("#form-content").val($("#editor-content").html());
                },
                beforeSubmit: function (arr, $form, option)
                {
                    //console.log(arr, $form, option);
                    //arr.push({ name: "Content", value: $("#editor-content").html() })
                    //$("#form-content").val($("#editor-content").html());
                    return true;
                },  // pre-submit callback
                success: function (data)
                {
                    if (data.success == true) {
                        $("#tab-task-" + data.id).load("/TaskRequest/Details/" + data.id, function ()
                        {
                            registerEditable($("#tab-task-" + data.id + ' .new-item-editable'));
                            //registerEditable($("#tab-task-" + data.id + ' .task-editable'));

                            registerBtnAddJob($("#tab-task-" + data.id + ' .add-item-btn'));
                        });
                        $('#request-modal').modal('hide');
                        //location.reload();
                    }
                }
            });
            //$('#request-from').datetimepicker({
            //    format: 'DD/MM/YYYY h:mm A'
            //});
            //$('#request-to').datetimepicker({
            //    format: 'DD/MM/YYYY h:mm A'
            //});
            $('#request-from').datetimepicker({
                format: 'd/m/Y h:i',
                lang: 'vi',
                //onShow: function (ct) {
                //    this.setOptions({
                //        maxDate: jQuery('#request-to').val() ? jQuery('#request-to').val() : false,
                //        formatDate: 'd/m/Y h:i A',
                //    })
                //}
            });
            $('#request-to').datetimepicker({
                format: 'd/m/Y H:i',
                lang: 'vi',
                //onShow: function (ct) {
                //    this.setOptions({
                //        minDate: jQuery('#request-from').val() ? jQuery('#request-from').val() : false,
                //        formatDate: 'd/m/Y h:i A',
                //    })
                //}
            });
            //$("#request-from").on("dp.change", function (e) {
            //    $('#request-to').data("DateTimePicker").minDate(e.date);
            //});
            //$("#request-to").on("dp.change", function (e) {
            //    $('#request-from').data("DateTimePicker").maxDate(e.date);
            //});
        });
    });

    $('#job-delete-modal, #request-delete-modal').on("show.bs.modal", function (e) {
        //var that = this;
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $(this).find("form").ajaxForm(function (data) {
                if (data.success == true) {
                    location.reload();
                }
            });
        });
    });

    $('#job-delete-modal').on("show.bs.modal", function (e) {
        //var that = this;
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $(this).find("form").ajaxForm(function (data) {
                if (data.success == true) {
                    $('#job-delete-modal').modal('hide');
                    $("#checklist-item-" + data.id).remove();
                }
            });
        });
    });

    $('#project-modal').on("show.bs.modal", function (e) {
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $("#tbEmployees").chosen({
                width: "100%",
                no_results_text: "Không có kết quả nào!"
            });

            $('#tbEmployees').on('change', function (evt, params) {
                var ids = "";
                var selectedIds = $("#tbEmployees").chosen().val();

                if (selectedIds != null) {
                    $.each(selectedIds, function (index, value) {
                        ids = ids + ", " + value;
                    });
                }
                $("#EmployeeIds").val(ids);
            });

            $('#attachments').ace_file_input({
                style: 'well',
                btn_choose: 'Drop files here or click to choose',
                btn_change: null,
                no_icon: 'ace-icon fa fa-cloud-upload',
                droppable: true,
                thumbnail: 'small'
                ,
                preview_error: function (filename, error_code) {
                }

            }).on('change', function () {
            });

            $('.uploaded-attachment .remove').on('click', function () {
                $(this).parent().remove();
            });
            $('#date-range').daterangepicker({
                'applyClass': 'btn-sm btn-success',
                'cancelClass': 'btn-sm btn-default',
                format: "DD/MM/YYYY",
                locale: {
                    applyLabel: 'Apply',
                    cancelLabel: 'Cancel',
                }
            });
            $('#date-range').on('apply.daterangepicker', function (ev, picker) {
                $("#Start").val(picker.startDate.format('DD/MM/YYYY'));
                $("#End").val(picker.endDate.format('DD/MM/YYYY'));
            });
            $(".project-form").ajaxForm(function (data) {
                if (data.success == true) {
                    location.reload();
                }
            });
        });
    });

    $('#project-delete-modal').on("show.bs.modal", function (e) {
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $("#project-delete-modal .project-form").ajaxForm(function (data) {
                if (data.success == true) {
                    location.reload();
                }
            });
        });
    });

    $('#response-delete-modal').on("show.bs.modal", function (e) {
        $(this).find(".modal-content").load($(e.relatedTarget).data('href'), function () {
            $("#response-delete-modal .response-form").ajaxForm(function (data) {
                if (data.success == true) {
                    location.reload();
                }
            });
        });
    });

    $('.modal').on("hidden.bs.modal", function (e) {
        $(this).find(".modal-content").empty();
    });

    $.fn.editableform.loading = "<div class='editableform-loading'><i class='ace-icon fa fa-spinner fa-spin fa-2x light-blue'></i></div>";
    $.fn.editableform.buttons = '<button type="submit" class="btn btn-info editable-submit"><i class="ace-icon fa fa-check"></i></button>' +
                                '<button type="button" class="btn editable-cancel"><i class="ace-icon fa fa-times"></i></button>';


})