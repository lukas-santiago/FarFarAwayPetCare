import { Button, Container } from 'react-bootstrap'
import { Link } from 'react-router-dom'
import { useContext } from 'react'
import GlobalContext from '../../contexts/global'

export function InicioRoute(): any {
  const { user, changeUser } = useContext(GlobalContext)

  return (
    <Container>
      <h1 className="mb-3">FarFarAway PetCare</h1>
      <hr />
      {!user && JSON.stringify(user) != '{}' ? (
        <Link to="/login">Login</Link>
      ) : (
        <Button onClick={() => changeUser(undefined)}>Logout</Button>
      )}
      <br />
      <Link to="/configuracao/dispositivos">Acessar Plataforma</Link>
      <br />
      <Link to="/sobre">Sobre</Link>
    </Container>
  )
}
