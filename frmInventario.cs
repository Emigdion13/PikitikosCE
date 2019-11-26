using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikitikosCE
{
    public partial class frmInventario : Form
    {
        pikitikosdbEntities db;
        bool IsLoading = true;

        //Make List observable collections???
        List<articulo> CacheArticulo;
        List<caracteristica> CacheCaracteristicas;
        List<color> CacheColor;
        List<talla> Cachetalla;
        List<tipoarticulo> CacheTipoArticulo;
        List<tipotela> CacheTipoTela;
        List<inventario> CacheInventario;

        List<articulo> DisplayArticulo;
        List<caracteristica> DisplayCaracteristicas;
        List<color> DisplayColor;
        List<talla> DisplayTalla;
        List<tipoarticulo> DisplayTipoArticulo;
        List<tipotela> DisplayTipoTela;
        List<inventario> DisplayInventario;

        public frmInventario()
        {
            InitializeComponent();

        }

        private void InventarioInsert()
        {
            string GenIDValue = "";

            try
            {
                this.toolStripStatusLabel1.Text = "Insertando a Inventario";

                articulo ArticuloaInsertar = CacheArticulo.First(x => x.Descripcion == "-");
                caracteristica CaracteristicaInsertar = CacheCaracteristicas.First(x => x.DescripcionCaracteristicas == "-");
                color ColorInsertar = CacheColor.First(x => x.DescripcionColor == "-");
                talla TallaInsertar = Cachetalla.First(x => x.DescriptionTalla == "-");
                tipoarticulo TipoArticuloInsertar = CacheTipoArticulo.First(x => x.DescripcionTipoArticulo == "-");
                tipotela TipoTelaInsertar = CacheTipoTela.First(x => x.DescripcionTela == "-");


                if (!string.IsNullOrEmpty(this.txtArticulo.Text.Trim()))
                {
                    ArticuloaInsertar = CacheArticulo.First(x => x.Descripcion == this.txtArticulo.Text.Trim());
                }

                if (!string.IsNullOrEmpty(this.txtCaracteristicas.Text.Trim()))
                {
                    CaracteristicaInsertar = CacheCaracteristicas.First(x => x.DescripcionCaracteristicas == this.txtCaracteristicas.Text.Trim());
                }

                if (!string.IsNullOrEmpty(this.txtColor.Text.Trim()))
                {
                    ColorInsertar = CacheColor.First(x => x.DescripcionColor == this.txtColor.Text.Trim());
                }

                if (!string.IsNullOrEmpty(this.txtTalla.Text.Trim()))
                {
                    TallaInsertar = Cachetalla.First(x => x.DescriptionTalla == this.txtTalla.Text.Trim());
                }

                if (!string.IsNullOrEmpty(this.txtTipoArticulo.Text.Trim()))
                {
                    TipoArticuloInsertar = CacheTipoArticulo.First(x => x.DescripcionTipoArticulo == this.txtTipoArticulo.Text.Trim());
                }

                if (!string.IsNullOrEmpty(this.txtTela.Text.Trim()))
                {
                    TipoTelaInsertar = CacheTipoTela.First(x => x.DescripcionTela == this.txtTela.Text.Trim());
                }

                txtPrecio_Leave(this, new EventArgs());

                string valor = this.txtPrecio.Text.Trim();
                float TransformedValue = 0f;

                if (!string.IsNullOrEmpty(valor))
                {
                    if (float.TryParse(valor, out TransformedValue))
                    {
                        if (TransformedValue <= 0) throw new Exception("El Precio debe ser mayor que 0");
                    } 
                }
                else
                {
                    throw new Exception("Indique un precio");
                }


                txtCantidad_Leave(this, new EventArgs());

                string valorCantidad = this.txtCantidad.Text.Trim();
                int TransformedValueCantidad = 0;

                if (!string.IsNullOrEmpty(valorCantidad))
                {
                    if (int.TryParse(valorCantidad, out TransformedValueCantidad))
                    {
                        if (TransformedValueCantidad < 0) throw new Exception("Debe ingresar al menos una cantidad de 0");
                    }
                }
                else
                {
                    throw new Exception("Indique una cantidad al menos de 0");
                }

                GenIDValue = string.Format("{0}{1}{2}{3}{4}{5}", ArticuloaInsertar.idArticulo, CaracteristicaInsertar.idCaracteristicas, ColorInsertar.idColor, TallaInsertar.idTalla, TipoArticuloInsertar.idTipoArticulo, TipoTelaInsertar.idTipoTela);
                var ItemExistente = db.inventarios.FirstOrDefault(x => x.GenId == GenIDValue);

                if (ItemExistente == default(inventario))
                {
                    //throw new Exception(string.Format("El producto con el id {0} ya esta en el sistema", GenIDValue));
                    var Item = db.inventarios.Add(new inventario() { GenId = GenIDValue, ArticuloLink = ArticuloaInsertar.idArticulo, CaracteristicasLink = CaracteristicaInsertar.idCaracteristicas, ColorLink = ColorInsertar.idColor, Disponible = TransformedValueCantidad, Precio = TransformedValue, TallaLink = TallaInsertar.idTalla, TipoArticuloLink = TipoArticuloInsertar.idTipoArticulo, TipoTelaLink = TipoTelaInsertar.idTipoTela });
                    CacheInventario.Add(Item);
                    

                    db.SaveChanges();

                    MessageBox.Show(string.Format("El Articulo con el codigo {0} ha Sido insertado", GenIDValue), "Insercion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tSLastStatus.Text = string.Format("El Articulo con el codigo {0} ha Sido insertado", GenIDValue);

                    

                } else
                {
                    ItemExistente.Disponible += TransformedValueCantidad;

                    db.SaveChanges();

                    MessageBox.Show(string.Format("El Articulo con el codigo {0} se ha actualizado {1} cantidad, Disponible: {2}", GenIDValue, TransformedValueCantidad, ItemExistente.Disponible), "Sumando inventario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tSLastStatus.Text = string.Format("El Articulo con el codigo {0} se ha actualizado {1} cantidad, Disponible: {2}", GenIDValue, TransformedValueCantidad, ItemExistente.Disponible);
                    
                }


            }
            catch(Exception ex)
             {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                db.Database.Connection.Close();
                this.toolStripStatusLabel1.Text = "Listo";
                
            }

        }


        private async void btnCrear_Click(object sender, EventArgs e)
        {
            string errorToDisplay = "";
            string Pregunta = "Esta seguro que desea Ingresar ";
            int Conteo = Pregunta.Length;
            string ObjetoAInsertar = "";

            if (rbInventario.Checked)
            {
                await Task.Run(() => InventarioInsert());

                return;

            }
            else if (rbArticulo.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtArticulo.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtArticulo.Text.Trim();
                    Pregunta += string.Format("el articulo {0} ?", ObjetoAInsertar);
                }
                else
                {
                    errorToDisplay = "El articulo no puede estar vacio";
                }

            }
            else if (rbCaracteristicas.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtCaracteristicas.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtCaracteristicas.Text.Trim();
                    Pregunta += string.Format("la caracteristica {0} ?", ObjetoAInsertar);

                }
                else
                {
                    errorToDisplay = "Caracteristica no puede estar vacio";
                }

            }
            else if (rbColor.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtColor.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtColor.Text.Trim();
                    Pregunta += string.Format("el color {0} ?", ObjetoAInsertar);

                }
                else
                {
                    errorToDisplay = "Color no puede estar vacio";
                }

            }
            else if (rbTalla.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtTalla.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtTalla.Text.Trim();
                    Pregunta += string.Format("La talla {0} ?", ObjetoAInsertar);

                }
                else
                {
                    errorToDisplay = "Talla no puede estar vacio";
                }

            }
            else if (rbTipoArticulo.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtTipoArticulo.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtTipoArticulo.Text.Trim();
                    Pregunta += string.Format("El Tipo Articulo {0} ?", ObjetoAInsertar);

                }
                else
                {
                    errorToDisplay = "El tipo articulo no puede estar vacio";
                }

            }
            else if (rbTela.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtTela.Text.Trim()))
                {
                    ObjetoAInsertar = this.txtTela.Text.Trim();
                    Pregunta += string.Format("El Tipo tela {0} ?", ObjetoAInsertar);

                }
                else
                {
                    errorToDisplay = "El tipo tela no puede estar vacio";
                }

            }
            else
            {
                MessageBox.Show("Nada que hacer");
                return;
            }


            if (Conteo != Pregunta.Length)
            {
                if (DialogResult.Yes == MessageBox.Show(Pregunta, "Insercion", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        if (rbArticulo.Checked)
                        {
                            bool x = await Task.Run(() => db.articuloes.Any(x => x.Descripcion.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "El Articulo ya existe";
                            }
                            else
                            {
                                var Changes = db.articuloes.Add(new articulo() { Descripcion = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) CacheArticulo.Add(Changes);

                            }
                        }
                        else if (rbCaracteristicas.Checked)
                        {
                            bool x = await Task.Run(() => db.caracteristicas.Any(x => x.DescripcionCaracteristicas.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "La Caracteristica ya existe";
                            }
                            else
                            {
                                var Changes = db.caracteristicas.Add(new caracteristica() { DescripcionCaracteristicas = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) CacheCaracteristicas.Add(Changes);

                            }
                        }
                        else if (rbColor.Checked)
                        {
                            bool x = await Task.Run(() => db.colors.Any(x => x.DescripcionColor.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "El color ya existe";
                            }
                            else
                            {
                                var Changes = db.colors.Add(new color() { DescripcionColor = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) CacheColor.Add(Changes);

                            }
                        }
                        else if (rbTalla.Checked)
                        {
                            bool x = await Task.Run(() => db.tallas.Any(x => x.DescriptionTalla.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "La talla ya existe";
                            }
                            else
                            {
                                var Changes = db.tallas.Add(new talla() { DescriptionTalla = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) Cachetalla.Add(Changes);

                            }
                        }
                        else if (rbTipoArticulo.Checked)
                        {
                            bool x = await Task.Run(() => db.tipoarticuloes.Any(x => x.DescripcionTipoArticulo.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "El Tipo de arcitulo ya existe";
                            }
                            else
                            {
                                var Changes = db.tipoarticuloes.Add(new tipoarticulo() { DescripcionTipoArticulo = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) CacheTipoArticulo.Add(Changes);

                            }
                        }
                        else if (rbTela.Checked)
                        {
                            bool x = await Task.Run(() => db.tipotelas.Any(x => x.DescripcionTela.ToLower().Trim() == ObjetoAInsertar.ToLower()));

                            if (x)
                            {
                                errorToDisplay = "El Tipo de tela ya existe";
                            }
                            else
                            {
                                var Changes = db.tipotelas.Add(new tipotela() { DescripcionTela = ObjetoAInsertar });
                                int Resp = await db.SaveChangesAsync();
                                if (Resp == 1) CacheTipoTela.Add(Changes);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorToDisplay = ex.Message;
                    }
                    finally
                    {
                        db.Database.Connection.Close();

                    }
                }
                else
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(errorToDisplay))
            {
                MessageBox.Show(errorToDisplay, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Insertado!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private async Task InsertNullDummies()
        {
            try
            {
                if (!CacheArticulo.Any(x => x.Descripcion == "-")){
                    var Changes = db.articuloes.Add(new articulo() { Descripcion = "-" });
                     CacheArticulo.Add(Changes);
                }

                if (!CacheCaracteristicas.Any(x => x.DescripcionCaracteristicas == "-"))
                {
                    var Changes = db.caracteristicas.Add(new caracteristica() { DescripcionCaracteristicas = "-" });
                    CacheCaracteristicas.Add(Changes);
                }

                if (!CacheColor.Any(x => x.DescripcionColor == "-"))
                {
                    var Changes = db.colors.Add(new color() { DescripcionColor = "-" });
                    CacheColor.Add(Changes);
                }

                if (!Cachetalla.Any(x => x.DescriptionTalla == "-"))
                {
                    var Changes = db.tallas.Add(new talla() { DescriptionTalla = "-" });
                    Cachetalla.Add(Changes);
                }

                if (!CacheTipoArticulo.Any(x => x.DescripcionTipoArticulo == "-"))
                {
                    var Changes = db.tipoarticuloes.Add(new tipoarticulo() { DescripcionTipoArticulo = "-" });
                    CacheTipoArticulo.Add(Changes);
                }

                if (!CacheTipoTela.Any(x => x.DescripcionTela == "-"))
                {
                    var Changes = db.tipotelas.Add(new tipotela() { DescripcionTela = "-" });
                    CacheTipoTela.Add(Changes);
                }

                int Resp = await db.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private async void frmInventario_Load(object sender, EventArgs e)
        {

            db = new pikitikosdbEntities();
            CacheDbs();
            //  rbArticulo_CheckedChanged(this, new EventArgs());


            dgInventario.AutoGenerateColumns = true;
            dgInventario.DataSource = DisplayInventario;

            dgInventario.Columns[0].ReadOnly = true;
            dgInventario.Columns[0].Visible = false;
            dgInventario.Columns[1].ReadOnly = true;
            dgInventario.Columns[11].Visible = false;
            dgInventario.Columns[12].Visible = false;
            dgInventario.Columns[13].Visible = false;
            dgInventario.Columns[14].Visible = false;
            dgInventario.Columns[15].Visible = false;
            dgInventario.Columns[16].Visible = false;


            await InsertNullDummies();
        }

        private void CacheDbs()
        {
            try
            {
                IsLoading = true;

                /* this.toolStripStatusLabel1.Text = "Cargando Articulo en cache";
                 CacheArticulo = await Task.Run(() =>  db.articuloes.ToList());
                 this.toolStripStatusLabel1.Text = "Listo";

                 this.toolStripStatusLabel1.Text = "Cargando Caracteristica en cache";
                 CacheCaracteristicas = await Task.Run(() => db.caracteristicas.ToList());
                 this.toolStripStatusLabel1.Text = "Listo";

                 this.toolStripStatusLabel1.Text = "Cargando Color en cache";
                 CacheCaracteristicas = await Task.Run(() => db.caracteristicas.ToList());
                 this.toolStripStatusLabel1.Text = "Listo";*/

                this.toolStripStatusLabel1.Text = "Cargando la base de datos";
                Task.WaitAll(
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheArticulo = new pikitikosdbEntities().articuloes.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheCaracteristicas = new pikitikosdbEntities().caracteristicas.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheColor = new pikitikosdbEntities().colors.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { Cachetalla = new pikitikosdbEntities().tallas.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheTipoArticulo = new pikitikosdbEntities().tipoarticuloes.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheTipoTela = new pikitikosdbEntities().tipotelas.ToList(); } }),
                    Task.Run(() => { using (var c = new pikitikosdbEntities()) { CacheInventario = new pikitikosdbEntities().inventarios.ToList(); } })
                    );
                this.toolStripStatusLabel1.Text = "Listo";

                BuildDdisplay();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
                db.Database.Connection.Close();
            }

        }

        private void BuildDdisplay()
        {
            DisplayArticulo = CacheArticulo;
            DisplayCaracteristicas = CacheCaracteristicas;
            DisplayColor = CacheColor;
            DisplayTalla = Cachetalla;
            DisplayTipoArticulo = CacheTipoArticulo;
            DisplayTipoTela = CacheTipoTela;
            DisplayInventario = CacheInventario;
        }

        private void rbArticulo_CheckedChanged(object sender, EventArgs e)
        {

            if (IsLoading) return;

            if (rbArticulo.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayArticulo;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }

            //this.dGridIndividuals.item = db.articuloes.Select(x => new { ID = x.idArticulo, Descripcion = x.Descripcion });
        }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
        }

        private void txtArticulo_TextChanged(object sender, EventArgs e)
        {

            DisplayArticulo = CacheArticulo.Where(x => x.Descripcion.ToLower().Contains(txtArticulo.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayArticulo;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;

            if (rbCaracteristicas.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayCaracteristicas;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void txtArticulo_Enter(object sender, EventArgs e)
        {
            this.rbArticulo.Checked = true;
        }

        private void txtCaracteristicas_Enter(object sender, EventArgs e)
        {
            this.rbCaracteristicas.Checked = true;
        }

        private void rbColor_CheckedChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;

            if (rbColor.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayColor;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void txtCaracteristicas_TextChanged(object sender, EventArgs e)
        {
            DisplayCaracteristicas = CacheCaracteristicas.Where(x => x.DescripcionCaracteristicas.ToLower().Contains(txtCaracteristicas.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayCaracteristicas;
        }

        private void rbTalla_CheckedChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;

            if (rbTalla.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayTalla;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void rbTipoArticulo_CheckedChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;

            if (rbTipoArticulo.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayTipoArticulo;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void rbTela_CheckedChanged(object sender, EventArgs e)
        {
            if (IsLoading) return;

            if (rbTela.Checked)
            {

                this.dGridIndividuals.AutoGenerateColumns = true;
                this.dGridIndividuals.DataSource = DisplayTipoTela;
                this.dGridIndividuals.Columns[0].HeaderText = "ID";
                this.dGridIndividuals.Columns[0].ReadOnly = true;
                this.dGridIndividuals.Columns[2].Visible = false;
                this.dGridIndividuals.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void txtColor_Enter(object sender, EventArgs e)
        {
            this.rbColor.Checked = true;
        }

        private void txtTalla_Enter(object sender, EventArgs e)
        {
            this.rbTalla.Checked = true;
        }

        private void txtTipoArticulo_Enter(object sender, EventArgs e)
        {
            this.rbTipoArticulo.Checked = true;
        }

        private void txtTela_Enter(object sender, EventArgs e)
        {
            this.rbTela.Checked = true;
        }

        private void txtColor_TextChanged(object sender, EventArgs e)
        {
            DisplayColor = CacheColor.Where(x => x.DescripcionColor.ToLower().Contains(txtColor.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayColor;
        }

        private void txtTalla_TextChanged(object sender, EventArgs e)
        {
            DisplayTalla = Cachetalla.Where(x => x.DescriptionTalla.ToLower().Contains(txtTalla.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayTalla;
        }

        private void txtTipoArticulo_TextChanged(object sender, EventArgs e)
        {
            DisplayTipoArticulo = CacheTipoArticulo.Where(x => x.DescripcionTipoArticulo.ToLower().Contains(txtTipoArticulo.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayTipoArticulo;
        }

        private void txtTela_TextChanged(object sender, EventArgs e)
        {
            DisplayTipoTela = CacheTipoTela.Where(x => x.DescripcionTela.ToLower().Contains(txtTela.Text.ToLower())).ToList();
            this.dGridIndividuals.DataSource = DisplayTipoTela;
        }

        private void dGridIndividuals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (rbArticulo.Checked)
                {
                    this.txtArticulo.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if (rbCaracteristicas.Checked)
                {
                    this.txtCaracteristicas.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if (rbColor.Checked)
                {
                    this.txtColor.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if (rbTalla.Checked)
                {
                    this.txtTalla.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if (rbTipoArticulo.Checked)
                {
                    this.txtTipoArticulo.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
                else if (rbTela.Checked)
                {
                    this.txtTela.Text = dGridIndividuals.Rows[e.RowIndex].Cells[1].Value.ToString();
                } 
            }
        }

        private void rbInventario_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            string valor = this.txtPrecio.Text.Trim();
            float TransformedValue = 0f;

            if (!string.IsNullOrEmpty(valor))
            {
                if (!float.TryParse(valor, out TransformedValue))
                {
                    this.txtPrecio.Text = "-1";
                }
            }
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            string valor = this.txtCantidad.Text.Trim();
            int TransformedValue = 0;

            if (!string.IsNullOrEmpty(valor))
            {
                if (!int.TryParse(valor, out TransformedValue))
                {
                    this.txtPrecio.Text = "-1";
                }
            }
        }
    }
}
