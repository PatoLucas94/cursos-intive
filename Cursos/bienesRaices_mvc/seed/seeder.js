import {exit} from 'node:process'
import categorias from './categorias.js'
import precios from './precios.js'
import usuarios from './usuarios.js'
import db from '../config/db.js'
import {Categoria, Precio, Usuario} from '../models/index.js'

const importarDatos = async() => {
    try{
        // autenticar en la base de datos
        await db.authenticate()

        // Generar las columnas
        await db.sync()


        // Insertamos en la base de datos, TODOS los datos con bulkCreate
        
        await Promise.all([ // Cuando tenemos muchos await a la vez, estos inician al mismo tiempo. No tienen que esperar a que el otro termine. 
            await Categoria.bulkCreate(categorias),
            await Precio.bulkCreate(precios),
            await Usuario.bulkCreate(usuarios)
        ])

        

        console.log('Datos ingresados a la base')

        exit() // Termina la ejecucion de un proceso sin errores y asi no se queda consumiendo recursos


    }catch(error){
        console.log(error)
        exit(1)
    }
}

const eliminarDatos = async() => {

    try{

        await db.sync({force: true}) 

        // Es lo mismo que lo anterior pero en mas renglones
        
        /*await Promise.all([ 
            await Categoria.destroy({where: {}, truncate: true}),
            await Precio.destroy({where: {}, truncate: true})
        ])*/

        console.log('Datos Eliminados correctamente')
        exit()

    }catch(error){
        console.log(error)
        exit(1)
    }

}

// argv ---> Argumentos desde la linea de comandos, es un array
// "node ./seed/seeder.js -i"
// node: 1, pathDeCarpeta: 2, -i: 3
if(process.argv[2] === "-i"){
    importarDatos()
}

if(process.argv[2] === "-e"){
    eliminarDatos()
}