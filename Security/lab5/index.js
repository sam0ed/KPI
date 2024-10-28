require('dotenv').config()
const express = require('express')
const axios = require('axios')
const path = require('path')
const {auth} = require('express-oauth2-jwt-bearer')

const app = express()
const port = process.env.PORT || 3000

app.use(express.json())
app.use(express.urlencoded({extended: true}))

const checkJwt = auth({
    audience: process.env.AUDIENCE,
    issuerBaseURL: `https://${process.env.AUTH0_DOMAIN}/`,
})

app.get('/', async (req, res) => {
    res.sendFile(path.join(__dirname, 'index.html'))
})

app.get('/api/userinfo', checkJwt, async (req, res) => {
    const token = req.headers['authorization']
    try {
        const response = await axios({
            method: 'get',
            url: `https://${process.env.AUTH0_DOMAIN}/userinfo`,
            headers: {
                'content-type': 'application/json',
                Authorization: token,
            },
        })
        res.json({success: true, user: response.data})
    } catch (e) {
        console.log(e)
    }
})

app.post('/api/login', async (req, res) => {
    try {
        const {login, password} = req.body
        const response = await axios({
            method: 'post',
            url: `https://${process.env.AUTH0_DOMAIN}/oauth/token`,
            headers: {'content-type': 'application/x-www-form-urlencoded'},
            data: new URLSearchParams({
                grant_type: 'password',
                username: login,
                password: password,
                client_id: process.env.AUTH0_CLIENT_ID,
                client_secret: process.env.AUTH0_CLIENT_SECRET,
                audience: `https://${process.env.AUTH0_DOMAIN}/api/v2/`,
                scope: 'offline_access openid profile email',
                grant_type: 'http://auth0.com/oauth/grant-type/password-realm',
                realm: 'Username-Password-Authentication'
            }),
        })

        res.json({success: true, token: response.data.access_token})
    } catch (error) {
        console.error('Login failed:', error.response?.data || error.message)
        res.status(401).send('Login failed')
    }
})

app.listen(port, () => {
    console.log(`Example app listening on port ${port}`)
})
