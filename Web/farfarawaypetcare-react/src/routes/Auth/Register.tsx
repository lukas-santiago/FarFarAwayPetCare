import { Container, Card } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import { useState } from 'react'
import React from 'react'

export function RegisterRoute() {
  const [formData, setFormData] = useState({})

  return (
    <Container className="align-items-center d-flex h-100 justify-content-center">
      <Card>
        <article className="card-body">
          <div>
            Já tem uma conta?
            <div>
              <Link
                to="/login"
                className="float-right btn btn-outline-primary"
              >
                Entrar
              </Link>
            </div>
          </div>
          <hr />
          <h4 className="card-title mb-4 mt-1">Criar uma conta</h4>
          <form>
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
            <div className="form-group mt-3 d-flex justify-content-center">
              <button type="submit" className="btn btn-primary btn-block">
                Cadastrar
              </button>
            </div>
          </form>
        </article>
      </Card>
    </Container>
  )
}
