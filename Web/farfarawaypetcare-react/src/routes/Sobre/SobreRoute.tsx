import { useContext } from 'react'
import { Button, Container } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import GlobalContext from '../../contexts/global'

export function SobreRoute(): any {
  const { user, changeUser } = useContext(GlobalContext)

  return (
    <Container>
      <h1 className="mb-3">Sobre</h1>
      <hr />
      {!user ? (
        <Link to="/login">Login</Link>
      ) : (
        <Button onClick={() => changeUser(undefined)}>Logout</Button>
      )}
      <br />
      <Link to="/configuracao/dispositivos">Acessar Plataforma</Link>
    </Container>
  )
}
