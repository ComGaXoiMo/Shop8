var feedback = {
    init: function () {
        feedback.registerEvents();
    },
    registerEvents: function () {
        $('.custom-control-input').off('click').on('click', function () {
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Feedback/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response){
                    console.log(response);
                    if (response.status == true){
                        btn.change
                    }
                    else{
                        btn.change
                    }
                }
            });
        });
    }
}
feedback.init();