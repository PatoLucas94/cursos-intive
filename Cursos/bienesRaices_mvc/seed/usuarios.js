import bcrypt from 'bcrypt'

const usuarios = [
    {
        nombre: 'Patricio',
        email: 'pato@pato.gmail.com',
        confirmado: 1,
        password: bcrypt.hashSync('password', 10)
    }
]

export default usuarios