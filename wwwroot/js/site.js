

    function ShowModal444() {
        $('#AddValueModal').modal('show');
        $('#attmodaltitle').text('Add New Attribute');
};

function HideAttModal() {

    ClearAllControls();
    $('#AddValueModal').modal('hide');
};

function InsertAttValue() {

    var results = Validate();
    if (results==false) {
        return false;
    }
    var formData = new Object();
    formData.Id = $('#AttbributeId').val();
    formData.Values = $('#Attvalue').val();


    $.ajax({
        url: '/attributes/AddValues',
        data: formData,
        type: 'post',
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                alert("unable to save data");
            }
            else {
                HideAttModal();
                alert("Value Successfully Added");
                location.reload();
            }
        }
    });
}

function ClearAllControls() {
    $('#Attvalue').val('');
}

function Validate() {
    var isValid = true;

    if ($('#Attvalue').val().trim() == "") {
        $('#Attvalue').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Attvalue').css('border-color', 'Light-grey');
    }
    return isValid;
}

$('#Attvalue').on('change', function () {
    Validate();
})

function EditAttrValue(item) {
    $('#AddValueModal').modal('show');
    $('#attmodaltitle').text('Edit Attribute Value');
    $('#AttvalueReadonly').css('display', 'block');
    $('#AttvalueReadonlylb').css('display', 'block');
    $('#AttvalueReadonly').val(item);
    $('#SaveAtt').css('display', 'none');
    $('#UpdateAtt').css('display', '');
   

  



};

function UpdateAttValue() {
    var formData = new Object();

    formData.OldValue = $('#AttvalueReadonly').val();
    formData.NewValue = $('#Attvalue').val();
    formData.Id = $('#AttbributeId').val();
        

    $.ajax({
        url:'/Attributes/EditValues',
        data:formData,
        type:'post',
        success: function(response) {
            if (response == null || response == undefined || response.length == 0) {
                alert("unable to save data");
            }
            else {
                HideAttModal();
                alert('Data Updated Successfully');
                location.reload();
            }
        }
    })
       
};

function DeleteAttr(item) {
    $('#AddValueModal').modal('show');
    $('#attmodaltitle').text('Are You Sure You Want to Delete this Attribute');
    $('#AttvalueReadonly').css('display', 'block');
    $('#AttvalueReadonlylb').css('display', 'block');
    $('#AttvalueReadonly').val(item);
    $('#Attvalue').css('display', 'none');
    $('#RemoveAtt').css('display', '');
    $('#SaveAtt').css('display', 'none');
    $('#Attlabel').css('display', 'none');
    
    

}

function DeleteAtrrValue() {

    formData = new Object();
    formData.Data = $('#AttvalueReadonly').val();
    formData.Id = $('#AttbributeId').val();

    $.ajax({
        url: '/Attributes/RemoveValues',
        data: formData,
        type: 'post',
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                alert("unable to save data");
            }

            else {
                HideAttModal();
                alert(response);
                location.reload();
            }
        }
        
    })

};

