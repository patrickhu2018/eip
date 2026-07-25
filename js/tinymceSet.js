tinymce.init({
    language: "zh_TW",
    relative_urls: true,
    remove_script_host: false,
    convert_urls: true,
    selector: "textarea:not(.textarea-no-styles)",
    style_formats: [
            { title: 'Times New Roman', inline: 'span', styles: { 'font-family': 'Times New Roman' } },
            { title: 'Arial', inline: 'span', styles: { 'font-family': 'Arial' } },
            { title: 'Courier New', inline: 'span', styles: { 'font-family': 'Courier New' } },
            { title: '微軟正黑體', inline: 'span', styles: { 'font-family': '微軟正黑體' } },
            { title: '標楷體', inline: 'span', styles: { 'font-family': '標楷體' } },
            { title: '新細明體', inline: 'span', styles: { 'font-family': '新細明體' } },
            { title: '8px', inline: 'span', styles: { 'font-size': '8px' } },
            { title: '10px', inline: 'span', styles: { 'font-size': '10px' } },
            { title: '12px', inline: 'span', styles: { 'font-size': '12px' } },
            { title: '14px', inline: 'span', styles: { 'font-size': '14px' } },
            { title: '16px', inline: 'span', styles: { 'font-size': '16px' } },
            { title: '18px', inline: 'span', styles: { 'font-size': '18px' } },
            { title: '20px', inline: 'span', styles: { 'font-size': '20px' } }
    ],
    plugins: [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste",
        "textcolor colorpicker"
    ],
    toolbar: "undo redo | styleselect | bold italic | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist | link image"
    
});