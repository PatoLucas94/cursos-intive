import {Dropzone} from 'dropzone'

const token = document.querySelector('meta[name="csrf-token"]').getAttribute('content')

// El "imagen" es el id del form
Dropzone.options.imagen = {
    dictDefaultMessage: 'Sube tus imagenes aqui',
    acceptedFiles: '.png, .jpg, .jpeg',
    maxFileSize: 5, // megas
    maxFiles: 1,
    parallelUploads: 1,
    autoProccessQueue: false,
    addRemoveLinks: true,
    dictRemoveFile: 'Borrar Archivo',
    dictMaxFilesExceeded: 'El limite es 1 archivo',
    headers: { // Se envian antes del request
        'CSRF-Token': token
    },
    paramName: 'imagen',
    init: function(){ // Cuando inicie dropzone
        const dropzone = this
        const btnPublicar = document.querySelector('#publicar')

        // Una vez que se pongan todos los archivos, apretas el boton y comienza a procesarlos
        btnPublicar.addEventListener('click', function(){
            dropzone.proccessQueue()
        })

        dropzone.on('queuecomplete', function(file, mensaje){
            if(dropzone.getActiveFiles().length == 0){
                window.location.href = "/mis-propiedades"
            }
        })
    }
}()