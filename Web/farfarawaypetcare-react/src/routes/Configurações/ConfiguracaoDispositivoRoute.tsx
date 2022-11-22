import { useContext, useEffect, useState } from 'react'
import { Button, Card, Container } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { ErrorBoundary } from '../../components/ErrorBoundary'
import { GenericTable } from '../../components/GenericTable'
import GlobalContext from '../../contexts/global'
import { addDevice, getDevices } from '../../services/api'
import { Device } from '../../types/types'

const columns = ['#', 'Nome', 'ID do Dispositivo']

export function ConfiguracaoDispositivoRoute(): any {
  const ctx = useContext(GlobalContext)
  const navigate = useNavigate()
  const [deviceData, setDeviceData] = useState<Device[]>([])
  const [showDialog, setShowDialog] = useState<any>()
  const [formData, setFormData] = useState({
    nome: '',
    uniqueDeviceId: '',
    error: ''
  })

  const addDeviceHandler = async (e: any) => {
    e.preventDefault()
    try {
      await addDevice(ctx.user.token, {
        id: 0,
        nome: formData.nome,
        uniqueDeviceId: formData.uniqueDeviceId
      })
      setShowDialog(false)
      fetchData()
    } catch (error) {
      setFormData({
        ...formData,
        error: 'Erro ao adicionar, tente novamente.',
      })
    }
  }

  useEffect(() => {
    if (JSON.stringify(ctx.user) === JSON.stringify({})) navigate('/login')
    fetchData()
  }, [])

  function fetchData() {
    getDevices(ctx.user.token).then((res) => setDeviceData(res.data))

  }

  return (
    <Container>
      <h1 className="mb-3">Configuração dos Dispositivos</h1>
      <hr />
      <section style={{ position: 'relative' }}>
        <ErrorBoundary>
          <div className="d-flex justify-content-end">
            <Button size="sm" variant="secondary" onClick={() => setShowDialog(true)}>
              Adicionar Dispositivo
            </Button>
          </div>
          <GenericTable<Device>
            columns={columns}
            data={deviceData}
            actions={undefined}
            handleRow={(event: any, iRow: number) => {
              navigate('/configuracao/dispositivos/' + deviceData[iRow].id)
            }}
          />
        </ErrorBoundary>
        <dialog open={showDialog ? true : false} className='border-0 w-100'>
          <div className='modal-content modal-dialog-centered'>
            <form onSubmit={addDeviceHandler}>
              <Card>
                <Card.Header>
                  Adicionar
                </Card.Header>
                <Card.Body>
                  <div className="form-group">
                    <label>Nome</label>
                    <input
                      name="nome"
                      className="form-control"
                      type="text"
                      onChange={(event) => {
                        setFormData({
                          ...formData,
                          nome: event.target.value,
                        })
                      }}
                    />
                  </div>
                  <div className="form-group mt-3 ">
                    <label>ID do Dispositivo</label>
                    <input
                      className="form-control"
                      type="text"
                      onChange={(event) => {
                        setFormData({
                          ...formData,
                          uniqueDeviceId: event.target.value,
                        })
                      }}
                    />
                  </div>
                  <div className="text-danger mt-2">{formData.error}</div>
                </Card.Body>
                <Card.Footer className='d-flex gap-2 justify-content-center'>
                  <Button type='submit'>
                    Salvar
                  </Button>
                  <Button variant='danger' onClick={() => setShowDialog(false)}>
                    Cancelar
                  </Button>
                </Card.Footer>
              </Card>
            </form>
          </div>
        </dialog>
      </section>
    </Container>
  )
}
