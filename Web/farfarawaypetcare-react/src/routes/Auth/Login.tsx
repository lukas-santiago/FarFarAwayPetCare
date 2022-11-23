import './Login.css'
import { Card, Container } from 'react-bootstrap'
import { Link, useNavigate } from 'react-router-dom'
import { useContext, useState } from 'react'
import GlobalContext from '../../contexts/global'
import { postLogin } from '../../services/api'

export function LoginRoute() {
  const { user, changeUser } = useContext(GlobalContext)
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    error: '',
  })

  async function login(e: any) {
    e.preventDefault()
    try {
      let response : any = await postLogin(formData.username, formData.password)
      changeUser(response.data)
    } catch (error) {
      setFormData({
        ...formData,
        error: 'Credenciais inválidas, tente novamente.',
      })
    }
  }
  return (
    <>
      <Container className="align-items-center d-flex h-100 justify-content-center">
        <Card>
          <article className="card-body">
            <div>
              Não tem uma conta, ainda:
              <div>
                <Link
                  to="/register"
                  className="float-right btn btn-outline-primary"
                >
                  Criar uma conta
                </Link>
              </div>
            </div>
            <hr />
            <h4 className="card-title mb-4 mt-1">Entrar</h4>
            <form onSubmit={login}>
              <div className="form-group">
                <label>Usuário</label>
                <input
                  name=""
                  className="form-control"
                  placeholder="Ex: noobmaster69"
                  type="text"
                  onChange={(event) => {
                    setFormData({
                      ...formData,
                      username: event.target.value,
                    })
                  }}
                />
              </div>
              <div className="form-group mt-3 ">
                <label>Senha</label>
                <input
                  className="form-control"
                  placeholder="******"
                  type="password"
                  onChange={(event) => {
                    setFormData({
                      ...formData,
                      password: event.target.value,
                    })
                  }}
                />
              </div>
              <div className="text-danger mt-2">{formData.error}</div>
              <div className="form-group mt-3 d-flex justify-content-center">
                <button type="submit" className="btn btn-primary btn-block">
                  Login
                </button>
              </div>
            </form>
          </article>
        </Card>
      </Container>
    </>
  )
}
