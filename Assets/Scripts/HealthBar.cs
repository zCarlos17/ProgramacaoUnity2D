using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    [SerializeField] private float velocidade = 5f;
    [SerializeField] private TextMeshProUGUI vidaTexto;

    private Slider slider;
    private float alvo = 1f;

    void Awake()
    {
        instance = this;
        slider = GetComponent<Slider>();
        slider.value = 1f;
    }

    public void AtualizarVida(int vidaAtual, int vidaMaxima)
    {
        alvo = Mathf.Clamp01((float)vidaAtual / vidaMaxima);

        if (vidaTexto != null)
            vidaTexto.text = vidaAtual + " / " + vidaMaxima;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, alvo, velocidade * Time.deltaTime);
    }
}