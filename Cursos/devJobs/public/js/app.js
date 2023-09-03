import axios from 'axios'
import Swal from 'sweetalert2'

const skills = new Set()

document.addEventListener('DOMContentLoaded', () => {
    const skills = document.querySelector('.lista-conocimientos')

    // Limpiar las alertas
    let alertas = document.querySelector('.alertas')

    if(alertas){
        limpiarAlertas(alertas)
    }

    if(skills){
        skills.addEventListener('click', agregarSkills)

        // una vez que estemos en editar, llamemos a la funcion
        skillsSeleccionados()
    }

    const vacantesListado = document.querySelector('.panel-administracion')
    if(vacantesListado){
        vacantesListado.addEventListener('click', accionesListado)
    }
})

const agregarSkills = e => {
    if(e.target.tagName === 'LI'){
        if(e.target.classList.contains('activo')){
            skills.delete(e.target.textContent)
        e.target.classList.remove('activo')
        }else{
            skills.add(e.target.textContent)
            e.target.classList.add('activo')
        }
        
    }

    const skillsArray = [...skills]
    document.querySelector('#skills').value = skillsArray
}

const skillsSeleccionados = () => {
    // Viene en un NodeList, lo tansformas en un Array
    const seleccionadas = Array.from(document.querySelectorAll('.lista-conocimientos .activo'))

    seleccionadas.forEach(seleccionada => {
        skills.add(seleccionada.textContent)
    })

    //Inyectarlo en el hidden
    const skillsArray = [...skills]
    document.querySelector('#skills').value = skillsArray
}

const limpiarAlertas = (alertas) => {
    
    const interval = setInterval(() => {
        if(alertas.children.length > 0){
            alertas.removeChild(alertas.children[0])
        } else if(alertas.children.length === 0){
            alertas.parentElement.removeChild(alertas)
            clearInterval(interval)
        }
    }, 2000);
}

// Eliminar vacantes

const accionesListado = e => {
    e.preventDefault()

    // En el HTML esta data-eliminar... "data" es el dataset y "eliminar" es el nombre
    if(e.target.dataset.eliminar){

        // eliminar por medio de axios
        

        Swal.fire({

            title: 'Confirmar Eliminacion',
            text: "Una vez eliminada no se puede recuperar",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, Eliminar',
            cancelButtonText: 'No, Cancelar'

          }).then((result) => {

            if (result.isConfirmed) {
                // Eliminar la vacante por axios

                // location origin es el localhost actual
                // e.target.dataset.eliminar ---> es el id 
                const url = `${location.origin}/vacantes/eliminar/${e.target.dataset.eliminar}`

                // Axios para eliminar el registro
                axios.delete(url, {
                    params: {url}
                }).then(function(respuesta){
                    if(respuesta.status === 200){
                        Swal.fire(
                            'Eliminado!',
                            Respuesta.data,
                            'success'
                          )

                          // Eliminar del DOM
                          e.target.parentElement.parentElement.parentElement.removeChild(parentElement.parentElement)
                    }
                }).catch(() => {
                    Swal.fire({
                        type: 'error',
                        title: 'Hubo un error',
                        text: 'No se pudo Eliminar'
                    })
                })

              
            }
          })

    }else if(e.target.tagName === 'A'){
        // Si doy click en alguno de los otros enlaces, anda a esos enlaces
        window.location.href = e.target.href
    }
}